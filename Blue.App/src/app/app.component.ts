import { OAuthService } from 'angular-oauth2-oidc';
import { DxDataGridModule, DxButtonModule } from 'devextreme-angular';

import { JwksValidationHandler } from 'angular-oauth2-oidc';
import { authConfig } from '../shared/auth/modules/auth.config';
import { ImplicitFlowConfig } from '../shared/auth/modules/implicit-flow.config';
import { AuthPasswordFlowConfig } from './auth-password-flow.config';

import { Component, ViewChild } from '@angular/core';
import { Nav, Platform, MenuController, IonicPage } from 'ionic-angular';
import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { Network } from '@ionic-native/network';
import { Storage } from '@ionic/storage';
import { TranslateService } from 'ng2-translate';

import { SettingsComponent } from '../pages/settings/settings-component/settings.component';
import { LoginComponent } from '../pages/login/login-component/login.component';
import { HomeComponent } from '../pages/home/home-component/home.component';
import { DashboardComponent } from '../pages/dashboard/dashboard-component/dashboard.component';
import { DataGridComponent } from '../pages/data-grid/data-grid-component/data-grid.component';
import { ListComponent } from '../pages/list/list-component/list.component';

import { AuthService } from '../shared/services/auth.service';

@Component({
  templateUrl: 'app.html'
})
export class MyApp {
  @ViewChild(Nav) nav: Nav;

  rootPage: any;
  pages: Array<{ title: string, component: any, icon: string }>;

  isAuthenticate: boolean = false;

  constructor(private platform: Platform,
    private translate: TranslateService,
    private storage: Storage,
    private statusBar: StatusBar,
    private splashScreen: SplashScreen,
    private network: Network,
    private oauthService: OAuthService,
    private authService: AuthService,
    private menuController: MenuController) {

    //this.configurePasswordFlow();
    this.configureImplicitFlow();

    this.initializeApp();

    this.translate.setDefaultLang('en');
    storage.get('language').then((value) => {
      if (value) {
        this.translate.use(value);
      } else {
        this.translate.use('en');
        this.storage.set('language', 'en');
      }
    });

    // used for an example of ngFor and navigation
    this.pages = [
      { title: 'DASHBOARD', component: DashboardComponent, icon: 'home' },
      { title: 'Store Assignment', component: HomeComponent, icon: 'create' },
      { title: 'Item Request', component: HomeComponent, icon: 'list-box' },
      { title: 'Request Approval', component: HomeComponent, icon: 'done-all' },
      { title: 'Produce', component: HomeComponent, icon: 'hammer' },
      { title: 'Item Distribution', component: HomeComponent, icon: 'git-branch' },
      { title: 'Item Display', component: HomeComponent, icon: 'images' },
      { title: 'REPORT', component: HomeComponent, icon: 'stats' },
      { title: 'SETTINGS', component: SettingsComponent, icon: 'settings' },
      { title: 'LOGOUT', component: LoginComponent, icon: 'log-out' },
      { title: 'DataGrid', component: DataGridComponent, icon: 'home' },
      { title: 'List Example', component: ListComponent, icon: 'list-box' }
    ];
  }

  private configurePasswordFlow() {
    this.oauthService.configure(AuthPasswordFlowConfig);

    this.oauthService
      .events
      .filter(e => e.type == 'token_expires')
      .subscribe(e => {
        console.debug('received token_expires event', e);
        this.oauthService.refreshToken().then(() => {
          console.log('grant token');
        }).catch(() => {
          console.log('token expired');
        });
      });
  }

  private configureImplicitFlow() {
    this.oauthService.configure(authConfig);
    //this.oauthService.configure(ImplicitFlowConfig);
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();
    this.oauthService.loadDiscoveryDocument().then(() => {
      return this.authService.getTokenViaCode();
    }).then(result => {
      this.authService.tryLogin(null, result).then((result) => {
        this.isAuthenticate = true;
      });
    }).catch(error => {
      console.log("Discovery document load error!");
    });

    this.oauthService.events.subscribe(e => {
      console.log('oauth/oidc event', e);
    });

    this.oauthService.events.filter(e => e.type === 'session_terminated').subscribe(e => {
      console.log('Your session has been terminated!');
    });

    this.oauthService.events.filter(e => e.type === 'token_received').subscribe(e => {
      // this.oauthService.loadUserProfile();
      console.log('Token received');
    });

    this.oauthService
      .events
      .filter(e => e.type == 'token_expires')
      .subscribe(e => {
        console.debug('received token_expires event', e);
        this.oauthService.refreshToken().then(() => {
          console.log('grant token');
        }).catch(() => {
          console.log('token expired');
        });
      });
  }

  initializeApp() {
    this.platform.ready().then(() => {
      // Okay, so the platform is ready and our plugins are available.
      // Here you can do any higher level native things you might need.
      let disconnectSub = this.network.onDisconnect().subscribe(() => {
        console.log('you are offline');
      });
      
      let connectSub = this.network.onConnect().subscribe(()=> {
        console.log('you are online');
      });

      this.statusBar.styleDefault();
      this.splashScreen.hide();

      if (this.oauthService.hasValidIdToken() || this.oauthService.hasValidAccessToken()) {
        this.rootPage = HomeComponent;
      }
      else {
        this.rootPage = LoginComponent;
      }
    });
  }

  openPage(page) {
    // Reset the content nav to have just this page
    // we wouldn't want the back button to show in this scenario
    this.menuController.close();
    this.nav.setRoot(page.component);
  }
}
