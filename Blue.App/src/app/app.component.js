var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { OAuthService } from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc';
import { authConfig } from '../shared/auth/modules/auth.config';
import { AuthPasswordFlowConfig } from './auth-password-flow.config';
import { Component, ViewChild } from '@angular/core';
import { Nav, Platform, MenuController } from 'ionic-angular';
import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { Network } from '@ionic-native/network';
import { Storage } from '@ionic/storage';
import { TranslateService } from 'ng2-translate';
import { SettingsComponent } from '../pages/settings/settings-component/settings.component';
import { LoginComponent } from '../pages/login/login-component/login.component';
import { HomeComponent } from '../pages/home/home-component/home.component';
import { DashboardComponent } from '../pages/dashboard/dashboard-component/dashboard.component';
import { DataGridComponent } from "../pages/data-grid/data-grid-component/data-grid.component";
import { AuthService } from '../shared/services/auth.service';
var MyApp = (function () {
    function MyApp(platform, translate, storage, statusBar, splashScreen, network, oauthService, authService, menuController) {
        var _this = this;
        this.platform = platform;
        this.translate = translate;
        this.storage = storage;
        this.statusBar = statusBar;
        this.splashScreen = splashScreen;
        this.network = network;
        this.oauthService = oauthService;
        this.authService = authService;
        this.menuController = menuController;
        this.isAuthenticate = false;
        //this.configurePasswordFlow();
        this.configureImplicitFlow();
        this.initializeApp();
        this.translate.setDefaultLang('en');
        storage.get('language').then(function (value) {
            if (value) {
                _this.translate.use(value);
            }
            else {
                _this.translate.use('en');
                _this.storage.set('language', 'en');
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
            { title: 'DataGrid', component: DataGridComponent, icon: 'home' }
        ];
    }
    MyApp.prototype.configurePasswordFlow = function () {
        var _this = this;
        this.oauthService.configure(AuthPasswordFlowConfig);
        this.oauthService
            .events
            .filter(function (e) { return e.type == 'token_expires'; })
            .subscribe(function (e) {
            console.debug('received token_expires event', e);
            _this.oauthService.refreshToken().then(function () {
                console.log('grant token');
            }).catch(function () {
                console.log('token expired');
            });
        });
    };
    MyApp.prototype.configureImplicitFlow = function () {
        var _this = this;
        this.oauthService.configure(authConfig);
        //this.oauthService.configure(ImplicitFlowConfig);
        this.oauthService.tokenValidationHandler = new JwksValidationHandler();
        this.oauthService.loadDiscoveryDocument().then(function () {
            return _this.authService.getTokenViaCode();
        }).then(function (result) {
            _this.authService.tryLogin(null, result).then(function (result) {
                _this.isAuthenticate = true;
            });
        }).catch(function (error) {
            console.log("Discovery document load error!");
        });
        this.oauthService.events.subscribe(function (e) {
            console.log('oauth/oidc event', e);
        });
        this.oauthService.events.filter(function (e) { return e.type === 'session_terminated'; }).subscribe(function (e) {
            console.log('Your session has been terminated!');
        });
        this.oauthService.events.filter(function (e) { return e.type === 'token_received'; }).subscribe(function (e) {
            // this.oauthService.loadUserProfile();
            console.log('Token received');
        });
        this.oauthService
            .events
            .filter(function (e) { return e.type == 'token_expires'; })
            .subscribe(function (e) {
            console.debug('received token_expires event', e);
            _this.oauthService.refreshToken().then(function () {
                console.log('grant token');
            }).catch(function () {
                console.log('token expired');
            });
        });
    };
    MyApp.prototype.initializeApp = function () {
        var _this = this;
        this.platform.ready().then(function () {
            // Okay, so the platform is ready and our plugins are available.
            // Here you can do any higher level native things you might need.
            var disconnectSub = _this.network.onDisconnect().subscribe(function () {
                console.log('you are offline');
            });
            var connectSub = _this.network.onConnect().subscribe(function () {
                console.log('you are online');
            });
            _this.statusBar.styleDefault();
            _this.splashScreen.hide();
            if (_this.oauthService.hasValidIdToken() || _this.oauthService.hasValidAccessToken()) {
                _this.rootPage = HomeComponent;
            }
            else {
                _this.rootPage = LoginComponent;
            }
        });
    };
    MyApp.prototype.openPage = function (page) {
        // Reset the content nav to have just this page
        // we wouldn't want the back button to show in this scenario
        this.menuController.close();
        this.nav.setRoot(page.component);
    };
    return MyApp;
}());
__decorate([
    ViewChild(Nav),
    __metadata("design:type", Nav)
], MyApp.prototype, "nav", void 0);
MyApp = __decorate([
    Component({
        templateUrl: 'app.html'
    }),
    __metadata("design:paramtypes", [Platform,
        TranslateService,
        Storage,
        StatusBar,
        SplashScreen,
        Network,
        OAuthService,
        AuthService,
        MenuController])
], MyApp);
export { MyApp };
//# sourceMappingURL=app.component.js.map