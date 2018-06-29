import { Component } from '@angular/core';
import { NavController, App, AlertController } from 'ionic-angular';
import { LoginComponent } from '../../login/login-component/login.component';

import { OAuthService } from 'angular-oauth2-oidc';
import { AuthService } from '../../../shared/services/auth.service';
import { SqliteService } from '../../../app/shared/services/sqlite.service';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomeComponent {

  constructor(private navCtrl: NavController,
    private alertCtrl: AlertController,
    private app: App,
    private oauthService: OAuthService,
    private authService: AuthService,
    private sqliteService: SqliteService) { }

  database(){
    this.sqliteService.createDatabase();
  }

  showInfo() {
    this.authService.showInfo().then((result) => {
      let alert = this.alertCtrl.create({
        title: 'Identity Info',
        subTitle: 'Name: ' + result,
        buttons: ['OK']
      });

      alert.present();
    });
  }

  logout() {
    // TODO: Remove API Token
    this.oauthService.logOut(true);
    this.navCtrl.setRoot(LoginComponent);
    this.navCtrl.popToRoot();
  }

  refreshToken() {
    this.oauthService.refreshToken().then((result) => {
      console.debug('Token refreshed!');
    })
  }

  public get idToken(){
    return this.oauthService.getIdToken();
  }

  public get accessToken() {
    return this.oauthService.getAccessToken();
  }

  public get idTokenExpiration() {
    return this.oauthService.getIdTokenExpiration();
  }

  public get accessTokenExpiration() {
    return this.oauthService.getAccessTokenExpiration();
  }

  get claims() {
    return this.oauthService.getIdentityClaims();
  }
}
