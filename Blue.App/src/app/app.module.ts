import { NgModule, ErrorHandler } from '@angular/core';
import { IonicApp, IonicModule, IonicErrorHandler, DeepLinkConfig } from 'ionic-angular';

import { OAuthModule } from 'angular-oauth2-oidc';
import { SharedModule } from './shared/shared.module'
import { LoginModule } from '../pages/login/login.module';
import { HomeModule } from '../pages/home/home.module';
import { DashboardModule } from '../pages/dashboard/dashboard.module';
import { SettingsModule } from '../pages/settings/settings.module';
import { DataGridModule } from '../pages/data-grid/data-grid.module';
import { ListModule } from '../pages/list/list.module';

import { APP_BASE_HREF, LocationStrategy, PathLocationStrategy } from '@angular/common';

import { MyApp } from './app.component';
import { AuthService } from '../shared/services/auth.service';
import { SqliteService } from '../app/shared/services/sqlite.service';
import { HomeComponent } from "../pages/home/home-component/home.component";
import { LoginComponent } from "../pages/login/login-component/login.component";
import { DataGridComponent } from "../pages/data-grid/data-grid-component/data-grid.component";

@NgModule({
  declarations: [
    MyApp
  ],
  imports: [
    OAuthModule.forRoot(),
    IonicModule.forRoot(MyApp, {}, {
      links: [
        // { component: LoginComponent, name: 'Login Page', segment: '' },
        // { component: HomeComponent, name: 'Home Page', segment: 'home' },
        // { component: DataGridComponent, name: 'DataGrid Page', segment: 'data-grid' }
      ]
    }),
    SharedModule,
    LoginModule,
    HomeModule,
    DashboardModule,
    SettingsModule,
    DataGridModule,
    ListModule
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp
  ],
  providers: [
    AuthService,
    SqliteService,
    { provide: ErrorHandler, useClass: IonicErrorHandler },
    { provide: LocationStrategy, useClass: PathLocationStrategy },
    { provide: APP_BASE_HREF, useValue: '/' }
  ]
})
export class AppModule {}
