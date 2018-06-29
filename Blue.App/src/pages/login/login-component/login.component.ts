import { Component, ViewChild } from '@angular/core';
import { URLSearchParams, } from '@angular/http';
import { Nav, NavController, AlertController, LoadingController } from 'ionic-angular';

import { Deeplinks } from '@ionic-native/deeplinks';

import { HomeComponent } from '../../home/home-component/home.component';

import { OAuthService } from 'angular-oauth2-oidc';
import { AuthService } from '../../../shared/services/auth.service';
import { LoginInputModel } from '../../../shared/models/login-input.model';

@Component({
  selector: 'page-login',
  templateUrl: './login.html'
})
export class LoginComponent {
  @ViewChild(Nav) nav: Nav;

  loginInputModel: LoginInputModel = {
    grant_type: '',
    username: 'brian',
    password: '1qazZAQ!',
    client_id: '',
    client_secret: '',
    scope: ''
  };

  // We inject the router via DI
  constructor(public navCtrl: NavController,
            public alertCtrl: AlertController,
            public loadingCtrl: LoadingController,
            public oAuthService: OAuthService,
            public authService: AuthService,
            public deeplinks: Deeplinks) { }

  basedToken() {
    // This will be called when the user clicks on the Login button
    if (this.loginInputModel.username === '' || this.loginInputModel.password === '') {
      let alert = this.alertCtrl.create({
        title: 'Login Error',
        subTitle: 'All fields are required',
        buttons: ['OK']
      });

      alert.present();
      return;
    }

    let loader = this.loadingCtrl.create({
      content: 'Loading...'
    });

    loader.present();

    this.authService.basedToken(this.loginInputModel).then((result) => {
      console.log(result);
      loader.dismissAll();
      this.navCtrl.setRoot(HomeComponent);
    }, (error) => {
      loader.dismissAll();

      let errors = '';

      if (error.status === 0) errors += 'Connection Error!<br/>';
      else { errors += error.json().error; }

      let alert = this.alertCtrl.create({
        title: 'Login Error',
        subTitle: errors,
        buttons: ['OK']
      });
      alert.present();
    });
  }

  ownerPassword() {
    // This will be called when the user clicks on the Login button
    if (this.loginInputModel.username === '' || this.loginInputModel.password === '') {
      let alert = this.alertCtrl.create({
        title: 'Login Error',
        subTitle: 'All fields are required',
        buttons: ['OK']
      });

      alert.present();
      return;
    }

    let loader = this.loadingCtrl.create({
      content: 'Loading...'
    });

    loader.present();

    //this.authService.oAuthToken(this.loginInputModel).then((result) => {
    this.oAuthService.fetchTokenUsingPasswordFlow(this.loginInputModel.username, this.loginInputModel.password).then((result) => {
      console.log(result);
      loader.dismissAll();
      this.navCtrl.setRoot(HomeComponent);
    }, (error) => {
      loader.dismissAll();

      let errors = '';

      if (error.status === 0) errors += 'Connection Error!<br/>';
      else { errors += error.json().error; }

      let alert = this.alertCtrl.create({
        title: 'Login Error',
        subTitle: errors,
        buttons: ['OK']
      });
      alert.present();
    });
  }

  implicitFlow() {
    this.oAuthService.initImplicitFlow();
  }

  hybridFlow() {
    this.authService.initHybridFlow();
  }

  redirectHome() {
    this.navCtrl.setRoot(HomeComponent);
  }

  public get accessToken() {
    return this.oAuthService.getAccessToken();
  }

  logout() {
    // TODO: Remove API Token
    this.oAuthService.logOut(true);
    this.nav.setRoot(LoginComponent);
  }
}
