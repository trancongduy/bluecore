var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
import { Injectable, Optional } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { OAuthService, OAuthStorage, UrlHelperService } from 'angular-oauth2-oidc';
import { OAuthErrorEvent, OAuthSuccessEvent } from '../../shared/auth/auth-events';
import 'rxjs/add/operator/map';
import { Subject } from "rxjs/Subject";
import { Config } from '../../app/app.config';
var AuthService = (function () {
    function AuthService(http, oAuthService, storage, urlHelper, config) {
        this.http = http;
        this.oAuthService = oAuthService;
        this.urlHelper = urlHelper;
        this.config = config;
        this.eventsSubject = new Subject();
        console.log('Hello AuthService Provider');
        this.events = this.eventsSubject.asObservable();
        if (storage) {
            this.setStorage(storage);
        }
        else if (typeof sessionStorage !== 'undefined') {
            this.setStorage(sessionStorage);
        }
    }
    AuthService.prototype.helloWorld = function () {
        return 'Hello World!';
    };
    AuthService.prototype.configChanged = function () {
    };
    /**
       * Sets a custom storage used to store the received
       * tokens on client side. By default, the browser's
       * sessionStorage is used.
       *
       * @param storage
       */
    AuthService.prototype.setStorage = function (storage) {
        this._storage = storage;
        this.configChanged();
    };
    AuthService.prototype.validateUrlForHttps = function (url) {
        if (!url)
            return true;
        var lcUrl = url.toLowerCase();
        if (this.oAuthService.requireHttps === false)
            return true;
        if ((lcUrl.match(/^http:\/\/localhost($|[:\/])/)
            || lcUrl.match(/^http:\/\/localhost($|[:\/])/))
            && this.oAuthService.requireHttps === 'remoteOnly') {
            return true;
        }
        return lcUrl.startsWith('https://');
    };
    AuthService.prototype.createLoginUrl = function (state, loginHint, customRedirectUri, noPrompt, params) {
        var _this = this;
        if (state === void 0) { state = ''; }
        if (loginHint === void 0) { loginHint = ''; }
        if (customRedirectUri === void 0) { customRedirectUri = ''; }
        if (noPrompt === void 0) { noPrompt = false; }
        if (params === void 0) { params = {}; }
        var that = this;
        var redirectUri;
        if (customRedirectUri) {
            redirectUri = customRedirectUri;
        }
        else {
            redirectUri = this.oAuthService.redirectUri;
        }
        return this.oAuthService.createAndSaveNonce().then(function (nonce) {
            if (state) {
                state = nonce + ';' + state;
            }
            else {
                state = nonce;
            }
            if (!_this.oAuthService.requestAccessToken && !_this.oAuthService.oidc) {
                throw new Error('Either requestAccessToken or oidc or both must be true');
            }
            if (_this.oAuthService.oidc && _this.oAuthService.requestAccessToken) {
                _this.oAuthService.responseType = 'id_token code';
            }
            else if (_this.oAuthService.oidc && !_this.oAuthService.requestAccessToken) {
                _this.oAuthService.responseType = 'id_token';
            }
            else {
                _this.oAuthService.responseType = 'token';
            }
            var seperationChar = (that.oAuthService.loginUrl.indexOf('?') > -1) ? '&' : '?';
            var scope = that.oAuthService.scope;
            if (_this.oAuthService.oidc && !scope.match(/(^|\s)openid($|\s)/)) {
                scope = 'openid ' + scope;
            }
            var url = that.oAuthService.loginUrl
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
            for (var _i = 0, _a = Object.keys(params); _i < _a.length; _i++) {
                var key = _a[_i];
                url += '&' + encodeURIComponent(key) + '=' + encodeURIComponent(params[key]);
            }
            if (_this.oAuthService.customQueryParams) {
                for (var _b = 0, _c = Object.getOwnPropertyNames(_this.oAuthService.customQueryParams); _b < _c.length; _b++) {
                    var key = _c[_b];
                    url += '&' + key + '=' + encodeURIComponent(_this.oAuthService.customQueryParams[key]);
                }
            }
            return url;
        });
    };
    ;
    AuthService.prototype.callOnTokenReceivedIfExists = function (options) {
        var that = this;
        if (options.onTokenReceived) {
            var tokenParams = {
                idClaims: that.oAuthService.getIdentityClaims(),
                idToken: that.oAuthService.getIdToken(),
                accessToken: that.oAuthService.getAccessToken(),
                state: that.oAuthService.state
            };
            options.onTokenReceived(tokenParams);
        }
    };
    AuthService.prototype.validateNonceForAccessToken = function (accessToken, nonceInState) {
        var savedNonce = this._storage.getItem('nonce');
        if (savedNonce !== nonceInState) {
            var err = 'validating access_token failed. wrong state/nonce.';
            console.error(err, savedNonce, nonceInState);
            return false;
        }
        return true;
    };
    AuthService.prototype.storeAccessTokenResponse = function (accessToken, refreshToken, expiresIn) {
        this._storage.setItem('access_token', accessToken);
        this._storage.setItem('access_token_stored_at', '' + Date.now());
        if (expiresIn) {
            var expiresInMilliSeconds = expiresIn * 1000;
            var now = new Date();
            var expiresAt = now.getTime() + expiresInMilliSeconds;
            this._storage.setItem('expires_at', '' + expiresAt);
        }
        if (refreshToken) {
            this._storage.setItem('refresh_token', refreshToken);
        }
    };
    AuthService.prototype.storeIdToken = function (idToken) {
        this._storage.setItem('id_token', idToken.idToken);
        this._storage.setItem('id_token_claims_obj', idToken.idTokenClaimsJson);
        this._storage.setItem('id_token_expires_at', '' + idToken.idTokenExpiresAt);
        this._storage.setItem('id_token_stored_at', '' + Date.now());
    };
    AuthService.prototype.storeSessionState = function (sessionState) {
        this._storage.setItem('session_state', sessionState);
    };
    AuthService.prototype.getTokenViaCode = function () {
        var _this = this;
        return new Promise(function (resolve, reject) {
            var params = _this.urlHelper.getHashFragmentParams();
            var code = params['code'];
            if (!code)
                return resolve();
            var headers = new Headers({
                'Content-Type': 'application/x-www-form-urlencoded'
            });
            var body = new URLSearchParams();
            body.set('grant_type', 'authorization_code');
            body.set('code', code);
            body.set('client_id', 'mvc');
            body.set('client_secret', 'secret');
            body.set('redirect_uri', 'http://localhost:8100/index.html');
            _this.http.post(_this.config.idSvrUrl + '/connect/token', body.toString(), { headers: headers })
                .subscribe(function (result) {
                if (result) {
                    resolve(result.json());
                }
                else {
                    reject('error');
                }
            }, function (err) {
                reject(err);
            });
        });
    };
    ;
    AuthService.prototype.basedToken = function (credentials) {
        var _this = this;
        return new Promise(function (resolve, reject) {
            var headers = new Headers({
                'Content-Type': 'application/x-www-form-urlencoded'
            });
            var body = new URLSearchParams();
            body.set('username', credentials.username);
            body.set('password', credentials.password);
            _this.http.post(_this.config.apiUrl + '/auth/token', body.toString(), { headers: headers })
                .subscribe(function (token) {
                if (token) {
                    console.log(token.json());
                    localStorage.setItem('currentUser', token.json().access_token);
                    resolve(true);
                }
                else {
                    reject('incorrect');
                }
            }, function (err) {
                reject(err);
            });
        });
    };
    AuthService.prototype.oAuthToken = function (credentials) {
        var _this = this;
        return new Promise(function (resolve, reject) {
            var headers = new Headers({
                'Content-Type': 'application/x-www-form-urlencoded'
            });
            var body = new URLSearchParams();
            body.set('grant_type', 'password');
            body.set('username', credentials.username);
            body.set('password', credentials.password);
            body.set('client_id', 'ro.client');
            body.set('client_secret', 'secret');
            body.set('scope', 'offline_access');
            _this.http.post(_this.config.idSvrUrl + '/connect/token', body.toString(), { headers: headers })
                .subscribe(function (token) {
                if (token) {
                    console.log(token.json());
                    localStorage.setItem('currentUser', token.json().access_token);
                    resolve(true);
                }
                else {
                    reject('incorrect');
                }
            }, function (err) {
                reject(err);
            });
        });
    };
    ;
    AuthService.prototype.showInfo = function () {
        var _this = this;
        return new Promise(function (resolve, reject) {
            var bearer = "Bearer " + _this.oAuthService.getAccessToken();
            var headers = new Headers({
                'Content-Type': 'application/json',
                'Authorization': bearer
            });
            _this.http.get(_this.config.apiUrl + "/identity", { headers: headers })
                .map(function (response) { return response.json(); })
                .subscribe(function (res) {
                if (res) {
                    resolve(res);
                }
                else {
                    reject('incorrect');
                }
            }, function (err) {
                reject(err);
            });
        });
    };
    AuthService.prototype.initHybridFlow = function (additionalState, params) {
        if (additionalState === void 0) { additionalState = ''; }
        if (params === void 0) { params = ''; }
        if (!this.validateUrlForHttps(this.oAuthService.loginUrl)) {
            throw new Error('loginUrl must use Http. Also check property requireHttps.');
        }
        var addParams = {};
        var loginHint = null;
        if (typeof params === 'string') {
            loginHint = params;
        }
        else if (typeof params === 'object') {
            addParams = params;
        }
        this.createLoginUrl(additionalState, loginHint, null, false, addParams).then(function (url) {
            location.href = url;
        })
            .catch(function (error) {
            console.error('Error in initHybridFlow');
            console.error(error);
        });
    };
    ;
    AuthService.prototype.tryLogin = function (options, result) {
        var _this = this;
        if (options === void 0) { options = null; }
        if (result === void 0) { result = null; }
        options = options || {};
        var parts;
        if (options.customHashFragment) {
            parts = this.urlHelper.getHashFragmentParams(options.customHashFragment);
        }
        else {
            parts = this.urlHelper.getHashFragmentParams();
        }
        if (parts['error']) {
            //this.handleLoginError(options, parts);
            var err = new OAuthErrorEvent('token_error', {}, parts);
            this.eventsSubject.next(err);
            return Promise.reject(err);
        }
        var code = parts['code'];
        var accessToken = parts['access_token'];
        var idToken = parts['id_token'];
        var state = decodeURIComponent(parts['state']);
        var sessionState = parts['session_state'];
        if (!this.oAuthService.requestAccessToken && !this.oAuthService.oidc) {
            return Promise.reject('Either requestAccessToken or oidc or both must be true.');
        }
        if (this.oAuthService.requestAccessToken && !accessToken && code) {
            if (result) {
                accessToken = result.access_token;
                idToken = result.id_token;
            }
        }
        if (this.oAuthService.requestAccessToken && !accessToken && !code)
            return Promise.resolve();
        if (this.oAuthService.requestAccessToken && !options.disableOAuth2StateCheck && !state)
            return Promise.resolve();
        if (this.oAuthService.oidc && !idToken)
            return Promise.resolve();
        if (this.oAuthService.sessionChecksEnabled && !sessionState) {
            console.warn('session checks (Session Status Change Notification) '
                + 'is activated in the configuration but the id_token '
                + 'does not contain a session_state claim');
        }
        var stateParts = state.split(';');
        if (stateParts.length > 1) {
            this.oAuthService.state = stateParts[1];
        }
        var nonceInState = stateParts[0];
        if (this.oAuthService.requestAccessToken && !options.disableOAuth2StateCheck) {
            var success = this.validateNonceForAccessToken(accessToken, nonceInState);
            if (!success) {
                var event_1 = new OAuthErrorEvent('invalid_nonce_in_state', null);
                this.eventsSubject.next(event_1);
                return Promise.reject(event_1);
            }
        }
        if (this.oAuthService.requestAccessToken) {
            var expiresIn = parts['expires_in'];
            var refreshToken = parts['refresh_token'];
            if (result) {
                expiresIn = result.expires_in;
                refreshToken = result.refresh_token;
            }
            this.storeAccessTokenResponse(accessToken, refreshToken, expiresIn);
        }
        if (!this.oAuthService.oidc)
            return Promise.resolve();
        return this.oAuthService.processIdToken(idToken, accessToken)
            .then(function (result) {
            if (options.validationHandler) {
                return options.validationHandler({
                    accessToken: accessToken,
                    idClaims: result.idTokenClaims,
                    idToken: result.idToken,
                    state: state
                }).then(function (_) { return result; });
            }
            return result;
        })
            .then(function (result) {
            _this.storeIdToken(result);
            _this.storeSessionState(sessionState);
            _this.eventsSubject.next(new OAuthSuccessEvent('token_received'));
            _this.callOnTokenReceivedIfExists(options);
            if (_this.oAuthService.clearHashAfterLogin) {
                location.href = location.origin;
            }
        })
            .catch(function (reason) {
            _this.eventsSubject.next(new OAuthErrorEvent('token_validation_error', reason));
            console.error('Error validating tokens');
            console.error(reason);
        });
    };
    ;
    return AuthService;
}());
AuthService = __decorate([
    Injectable(),
    __param(2, Optional()),
    __metadata("design:paramtypes", [Http,
        OAuthService,
        OAuthStorage,
        UrlHelperService,
        Config])
], AuthService);
export { AuthService };
//# sourceMappingURL=auth.service.js.map