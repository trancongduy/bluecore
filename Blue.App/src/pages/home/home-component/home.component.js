var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component } from '@angular/core';
import { NavController, App, AlertController } from 'ionic-angular';
import { LoginComponent } from '../../login/login-component/login.component';
import { OAuthService } from 'angular-oauth2-oidc';
import { AuthService } from '../../../shared/services/auth.service';
var HomeComponent = (function () {
    function HomeComponent(navCtrl, alertCtrl, app, oauthService, authService) {
        this.navCtrl = navCtrl;
        this.alertCtrl = alertCtrl;
        this.app = app;
        this.oauthService = oauthService;
        this.authService = authService;
    }
    HomeComponent.prototype.showInfo = function () {
        var _this = this;
        this.authService.showInfo().then(function (result) {
            var alert = _this.alertCtrl.create({
                title: 'Identity Info',
                subTitle: 'Name: ' + result,
                buttons: ['OK']
            });
            alert.present();
        });
    };
    HomeComponent.prototype.logout = function () {
        // TODO: Remove API Token
        this.oauthService.logOut(true);
        this.navCtrl.setRoot(LoginComponent);
        this.navCtrl.popToRoot();
    };
    HomeComponent.prototype.refreshToken = function () {
        this.oauthService.refreshToken().then(function (result) {
            console.debug('Token refreshed!');
        });
    };
    Object.defineProperty(HomeComponent.prototype, "idToken", {
        get: function () {
            return this.oauthService.getIdToken();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HomeComponent.prototype, "accessToken", {
        get: function () {
            return this.oauthService.getAccessToken();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HomeComponent.prototype, "idTokenExpiration", {
        get: function () {
            return this.oauthService.getIdTokenExpiration();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HomeComponent.prototype, "accessTokenExpiration", {
        get: function () {
            return this.oauthService.getAccessTokenExpiration();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HomeComponent.prototype, "claims", {
        get: function () {
            return this.oauthService.getIdentityClaims();
        },
        enumerable: true,
        configurable: true
    });
    return HomeComponent;
}());
HomeComponent = __decorate([
    Component({
        selector: 'page-home',
        templateUrl: 'home.html'
    }),
    __metadata("design:paramtypes", [NavController,
        AlertController,
        App,
        OAuthService,
        AuthService])
], HomeComponent);
export { HomeComponent };
//# sourceMappingURL=home.component.js.map