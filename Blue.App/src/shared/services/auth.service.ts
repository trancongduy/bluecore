import { Injectable, Optional } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { OAuthService, OAuthStorage, UrlHelperService, LoginOptions, ParsedIdToken } from 'angular-oauth2-oidc';
import { OAuthEvent, OAuthErrorEvent, OAuthSuccessEvent } from '../../shared/auth/auth-events';
import 'rxjs/add/operator/map';
import { Subject } from "rxjs/Subject";
import { Observable } from "rxjs/Observable";

import { Config } from '../../app/app.config';

@Injectable()
export class AuthService {
  private eventsSubject: Subject<OAuthEvent> = new Subject<OAuthEvent>();
  private _storage: OAuthStorage;

  /**
     * Informs about events, like token_received or token_expires.
     * See the string enum EventType for a full list of events.
     */
  public events: Observable<OAuthEvent>;

  constructor(public http: Http,
    public oAuthService: OAuthService,
    @Optional() storage: OAuthStorage,
    public urlHelper: UrlHelperService,
    private config: Config) {
    console.log('Hello AuthService Provider');

    this.events = this.eventsSubject.asObservable();

    if (storage) {
      this.setStorage(storage);
    } else if (typeof sessionStorage !== 'undefined') {
      this.setStorage(sessionStorage);
    }
  }

  helloWorld(){
    return 'Hello World!';
  }

  private configChanged(): void {
  }

  /**
     * Sets a custom storage used to store the received
     * tokens on client side. By default, the browser's
     * sessionStorage is used.
     *
     * @param storage
     */

  public setStorage(storage: OAuthStorage): void {
    this._storage = storage;
    this.configChanged();
  }


  private validateUrlForHttps(url: string): boolean {

    if (!url) return true;

    let lcUrl = url.toLowerCase();

    if (this.oAuthService.requireHttps === false) return true;

    if ((lcUrl.match(/^http:\/\/localhost($|[:\/])/)
      || lcUrl.match(/^http:\/\/localhost($|[:\/])/))
      && this.oAuthService.requireHttps === 'remoteOnly') {
      return true;
    }

    return lcUrl.startsWith('https://');
  }

  private createLoginUrl(
    state = '',
    loginHint = '',
    customRedirectUri = '',
    noPrompt = false,
    params: object = {}
  ) {
    let that = this;

    let redirectUri: string;

    if (customRedirectUri) {
      redirectUri = customRedirectUri;
    }
    else {
      redirectUri = this.oAuthService.redirectUri;
    }

    return this.oAuthService.createAndSaveNonce().then((nonce: any) => {

      if (state) {
        state = nonce + ';' + state;
      }
      else {
        state = nonce;
      }

      if (!this.oAuthService.requestAccessToken && !this.oAuthService.oidc) {
        throw new Error('Either requestAccessToken or oidc or both must be true');
      }

      if (this.oAuthService.oidc && this.oAuthService.requestAccessToken) {
        this.oAuthService.responseType = 'id_token code';
      }
      else if (this.oAuthService.oidc && !this.oAuthService.requestAccessToken) {
        this.oAuthService.responseType = 'id_token';
      }
      else {
        this.oAuthService.responseType = 'token';
      }

      let seperationChar = (that.oAuthService.loginUrl.indexOf('?') > -1) ? '&' : '?';

      let scope = that.oAuthService.scope;

      if (this.oAuthService.oidc && !scope.match(/(^|\s)openid($|\s)/)) {
        scope = 'openid ' + scope;
      }

      let url = that.oAuthService.loginUrl
        + seperationChar
        + 'response_type='
        + encodeURIComponent(that.oAuthService.responseType)
        + '&client_id='
        + encodeURIComponent(that.oAuthService.clientId)
        + '&state='
        + encodeURIComponent(state)
        + '&redirect_uri='
        + encodeURIComponent(redirectUri)
        + '&scope='
        + encodeURIComponent(scope);

      if (loginHint) {
        url += '&login_hint=' + encodeURIComponent(loginHint);
      }

      if (that.oAuthService.resource) {
        url += '&resource=' + encodeURIComponent(that.oAuthService.resource);
      }

      if (that.oAuthService.oidc) {
        url += '&nonce=' + encodeURIComponent(nonce);
      }

      if (noPrompt) {
        url += '&prompt=none';
      }

      for (let key of Object.keys(params)) {
        url += '&' + encodeURIComponent(key) + '=' + encodeURIComponent(params[key]);
      }

      if (this.oAuthService.customQueryParams) {
        for (let key of Object.getOwnPropertyNames(this.oAuthService.customQueryParams)) {
          url += '&' + key + '=' + encodeURIComponent(this.oAuthService.customQueryParams[key]);
        }
      }

      return url;
    });
  };

  private callOnTokenReceivedIfExists(options: LoginOptions): void {
    let that = this;
    if (options.onTokenReceived) {
      let tokenParams = {
        idClaims: that.oAuthService.getIdentityClaims(),
        idToken: that.oAuthService.getIdToken(),
        accessToken: that.oAuthService.getAccessToken(),
        state: that.oAuthService.state
      };
      options.onTokenReceived(tokenParams);
    }
  }

  private validateNonceForAccessToken(accessToken: string, nonceInState: string): boolean {
    let savedNonce = this._storage.getItem('nonce');
    if (savedNonce !== nonceInState) {
      let err = 'validating access_token failed. wrong state/nonce.';
      console.error(err, savedNonce, nonceInState);
      return false;
    }
    return true;
  }

  private storeAccessTokenResponse(accessToken: string, refreshToken: string, expiresIn: number): void {
    this._storage.setItem('access_token', accessToken);
    this._storage.setItem('access_token_stored_at', '' + Date.now());
    if (expiresIn) {
      let expiresInMilliSeconds = expiresIn * 1000;
      let now = new Date();
      let expiresAt = now.getTime() + expiresInMilliSeconds;
      this._storage.setItem('expires_at', '' + expiresAt);
    }

    if (refreshToken) {
      this._storage.setItem('refresh_token', refreshToken);
    }
  }

  protected storeIdToken(idToken: ParsedIdToken) {
    this._storage.setItem('id_token', idToken.idToken);
    this._storage.setItem('id_token_claims_obj', idToken.idTokenClaimsJson);
    this._storage.setItem('id_token_expires_at', '' + idToken.idTokenExpiresAt);
    this._storage.setItem('id_token_stored_at', '' + Date.now());
  }

  protected storeSessionState(sessionState: string): void {
    this._storage.setItem('session_state', sessionState);
  }

  public getTokenViaCode() {
    return new Promise((resolve, reject) => {
      let params = this.urlHelper.getHashFragmentParams();
      let code = params['code'];

      if (!code) return resolve();

      let headers = new Headers({
        'Content-Type': 'application/x-www-form-urlencoded'
      });

      let body = new URLSearchParams();
      body.set('grant_type', 'authorization_code');
      body.set('code', code);
      body.set('client_id', 'mvc');
      body.set('client_secret', 'secret');
      body.set('redirect_uri', 'http://localhost:8100/index.html');

      this.http.post(this.config.idSvrUrl + '/connect/token', body.toString(), { headers: headers })
        .subscribe(result => {
          if (result) {
            resolve(result.json());
          }
          else {
            reject('error');
          }
        }, (err) => {
          reject(err);
        });
    });
  };

  public basedToken(credentials) {
    return new Promise((resolve, reject) => {
      let headers = new Headers({
        'Content-Type': 'application/x-www-form-urlencoded'
      });

      let body = new URLSearchParams();
      body.set('username', credentials.username);
      body.set('password', credentials.password);

      this.http.post(this.config.apiUrl + '/auth/token', body.toString(), { headers: headers })
        .subscribe(token => {
          if (token) {
            console.log(token.json());
            localStorage.setItem('currentUser', token.json().access_token);
            resolve(true); 
          }
          else {
            reject('incorrect');
          }
        }, (err) => {
          reject(err);
        });
    });
  }

  public oAuthToken(credentials) {
    return new Promise((resolve, reject) => {
      let headers = new Headers({
        'Content-Type': 'application/x-www-form-urlencoded'
      });

      let body = new URLSearchParams();
      body.set('grant_type', 'password');
      body.set('username', credentials.username);
      body.set('password', credentials.password);
      body.set('client_id', 'ro.client');
      body.set('client_secret', 'secret');
      body.set('scope', 'offline_access');

      this.http.post(this.config.idSvrUrl + '/connect/token', body.toString(), { headers: headers })
        .subscribe(token => {
          if (token) {
            console.log(token.json());
            localStorage.setItem('currentUser', token.json().access_token);
            resolve(true);
          }
          else {
            reject('incorrect');
          }
        }, (err) => {
          reject(err);
        });
    });
  };

  public showInfo() {
    return new Promise((resolve, reject) => {
      var bearer = "Bearer " + this.oAuthService.getAccessToken();
      var headers = new Headers({
        'Content-Type': 'application/json',
        'Authorization': bearer
      });

      this.http.get(this.config.apiUrl + "/identity", { headers: headers })
        .map(response => response.json())
        .subscribe((res) => {
          if (res) {
            resolve(res);
          } else {
            reject('incorrect');
          }
        }, (err) => {
          reject(err);
        });
    });
  }

  public initHybridFlow(additionalState = '', params: string | object = ''): void {

    if (!this.validateUrlForHttps(this.oAuthService.loginUrl)) {
      throw new Error('loginUrl must use Http. Also check property requireHttps.');
    }

    let addParams: object = {};
    let loginHint: string = null;

    if (typeof params === 'string') {
      loginHint = params;
    }
    else if (typeof params === 'object') {
      addParams = params;
    }

    this.createLoginUrl(additionalState, loginHint, null, false, addParams).then(function (url) {
      location.href = url;
    })
      .catch(error => {
        console.error('Error in initHybridFlow');
        console.error(error);
      });
  };

  public tryLogin(options: LoginOptions = null, result = null): Promise<void> {

    options = options || {};

    let parts: object;

    if (options.customHashFragment) {
      parts = this.urlHelper.getHashFragmentParams(options.customHashFragment);
    }
    else {
      parts = this.urlHelper.getHashFragmentParams();
    }

    if (parts['error']) {
      //this.handleLoginError(options, parts);

      let err = new OAuthErrorEvent('token_error', {}, parts);
      this.eventsSubject.next(err);
      return Promise.reject(err);
    }

    let code = parts['code'];
    let accessToken = parts['access_token'];
    let idToken = parts['id_token'];
    let state = decodeURIComponent(parts['state']);
    let sessionState = parts['session_state'];

    if (!this.oAuthService.requestAccessToken && !this.oAuthService.oidc) {
      return Promise.reject('Either requestAccessToken or oidc or both must be true.');
    }
    if (this.oAuthService.requestAccessToken && !accessToken && code) {
      if (result) {
        accessToken = result.access_token;
        idToken = result.id_token;
      }
    }
    if (this.oAuthService.requestAccessToken && !accessToken && !code) return Promise.resolve();
    if (this.oAuthService.requestAccessToken && !options.disableOAuth2StateCheck && !state) return Promise.resolve();
    if (this.oAuthService.oidc && !idToken) return Promise.resolve();

    if (this.oAuthService.sessionChecksEnabled && !sessionState) {
      console.warn(
        'session checks (Session Status Change Notification) '
        + 'is activated in the configuration but the id_token '
        + 'does not contain a session_state claim');
    }

    let stateParts = state.split(';');
    if (stateParts.length > 1) {
      this.oAuthService.state = stateParts[1];
    }
    let nonceInState = stateParts[0];

    if (this.oAuthService.requestAccessToken && !options.disableOAuth2StateCheck) {
      let success = this.validateNonceForAccessToken(accessToken, nonceInState);
      if (!success) {
        let event = new OAuthErrorEvent('invalid_nonce_in_state', null);
        this.eventsSubject.next(event);
        return Promise.reject(event);
      }
    }

    if (this.oAuthService.requestAccessToken) {
      let expiresIn = parts['expires_in'];
      let refreshToken = parts['refresh_token'];
      if (result) {
        expiresIn = result.expires_in;
        refreshToken = result.refresh_token;
      }
      this.storeAccessTokenResponse(accessToken, refreshToken, expiresIn);
    }

    if (!this.oAuthService.oidc) return Promise.resolve();

    return this.oAuthService.processIdToken(idToken, accessToken)
      .then(result => {
        if (options.validationHandler) {
          return options.validationHandler({
            accessToken: accessToken,
            idClaims: result.idTokenClaims,
            idToken: result.idToken,
            state: state
          }).then(_ => result);
        }
        return result;
      })
      .then(result => {
        this.storeIdToken(result);
        this.storeSessionState(sessionState);
        this.eventsSubject.next(new OAuthSuccessEvent('token_received'));
        this.callOnTokenReceivedIfExists(options);
        if (this.oAuthService.clearHashAfterLogin) {
          location.href = location.origin;
        }
      })
      .catch(reason => {
        this.eventsSubject.next(new OAuthErrorEvent('token_validation_error', reason));
        console.error('Error validating tokens');
        console.error(reason);
      });

  };
}
