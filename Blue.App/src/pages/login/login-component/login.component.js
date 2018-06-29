var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ViewChild } from '@angular/core';
import { Nav, NavController, AlertController, LoadingController } from 'ionic-angular';
import { Deeplinks } from '@ionic-native/deeplinks';
import { HomeComponent } from '../../home/home-component/home.component';
import { OAuthService } from 'angular-oauth2-oidc';
import { AuthService } from '../../../shared/services/auth.service';
var LoginComponent = LoginComponent_1 = (function () {
    // We inject the router via DI
    function LoginComponent(navCtrl, alertCtrl, loadingCtrl, oAuthService, authService, deeplinks) {
        this.navCtrl = navCtrl;
        this.alertCtrl = alertCtrl;
        this.loadingCtrl = loadingCtrl;
        this.oAuthService = oAuthService;
        this.authService = authService;
        this.deeplinks = deeplinks;
        this.loginInputModel = {
            grant_type: '',
            username: 'brian',
            password: '1qazZAQ!',
            client_id: '',
            client_secret: '',
            scope: ''
        };
    }
    LoginComponent.prototype.basedToken = function () {
        var _this = this;
        // This will be called when the user clicks on the Login button
        if (this.loginInputModel.username === '' || this.loginInputModel.password === '') {
            var alert_1 = this.alertCtrl.create({
                title: 'Login Error',
                subTitle: 'All fields are required',
                buttons: ['OK']
            });
            alert_1.present();
            return;
        }
        var loader = this.loadingCtrl.create({
            content: 'Loading...'
        });
        loader.present();
        this.authService.basedToken(this.loginInputModel).then(function (result) {
            console.log(result);
            loader.dismissAll();
            _this.navCtrl.setRoot(HomeComponent);
        }, function (error) {
            loader.dismissAll();
            var errors = '';
            if (error.status === 0)
                errors += 'Connection Error!<br/>';
            else {
                errors += error.json().error;
            }
            var alert = _this.alertCtrl.create({
                title: 'Login Error',
                subTitle: errors,
                buttons: ['OK']
            });
            alert.present();
        });
    };
    LoginComponent.prototype.ownerPassword = function () {
        var _this = this;
        // This will be called when the user clicks on the Login button
        if (this.loginInputModel.username === '' || this.loginInputModel.password === '') {
            var alert_2 = this.alertCtrl.create({
                title: 'Login Error',
                subTitle: 'All fields are required',
                buttons: ['OK']
            });
            alert_2.present();
            return;
        }
        var loader = this.loadingCtrl.create({
            content: 'Loading...'
        });
        loader.present();
        //this.authService.oAuthToken(this.loginInputModel).then((result) => {
        this.oAuthService.fetchTokenUsingPasswordFlow(this.loginInputModel.username, this.loginInputModel.password).then(function (result) {
            console.log(result);
            loader.dismissAll();
            _this.navCtrl.setRoot(HomeComponent);
        }, function (error) {
            loader.dismissAll();
            var errors = '';
            if (error.status === 0)
                errors += 'Connection Error!<br/>';
            else {
                errors += error.json().error;
            }
            var alert = _this.alertCtrl.create({
                title: 'Login Error',
                subTitle: errors,
                buttons: ['OK']
            });
            alert.present();
        });
    };
    LoginComponent.prototype.implicitFlow = function () {
        this.oAuthService.initImplicitFlow();
    };
    LoginComponent.prototype.hybridFlow = function () {
        this.authService.initHybridFlow();
    };
    LoginComponent.prototype.redirectHome = function () {
        this.navCtrl.setRoot(HomeComponent);
    };
    Object.defineProperty(LoginComponent.prototype, "accessToken", {
        get: function () {
            return this.oAuthService.getAccessToken();
        },
        enumerable: true,
        configurable: true
    });
    LoginComponent.prototype.logout = function () {
        // TODO: Remove API Token
        this.oAuthService.logOut(true);
        this.nav.setRoot(LoginComponent_1);
    };
    return LoginComponent;
}());
__decorate([
    ViewChild(Nav),
    __metadata("design:type", Nav)
], LoginComponent.prototype, "nav", void 0);
LoginComponent = LoginComponent_1 = __decorate([
    Component({
        selector: 'page-login',
        templateUrl: './login.html'
    }),
    __metadata("design:paramtypes", [NavController,
        AlertController,
        LoadingController,
        OAuthService,
        AuthService,
        Deeplinks])
], LoginComponent);
export { LoginComponent };
var LoginComponent_1;
//# sourceMappingURL=login.component.js.map