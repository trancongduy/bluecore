var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { NgModule, ErrorHandler } from '@angular/core';
import { IonicApp, IonicModule, IonicErrorHandler } from 'ionic-angular';
import { OAuthModule } from 'angular-oauth2-oidc';
import { SharedModule } from './shared/shared.module';
import { LoginModule } from '../pages/login/login.module';
import { HomeModule } from '../pages/home/home.module';
import { DashboardModule } from '../pages/dashboard/dashboard.module';
import { SettingsModule } from '../pages/settings/settings.module';
import { DataGridModule } from "../pages/data-grid/data-grid.module";
import { APP_BASE_HREF, LocationStrategy, PathLocationStrategy } from '@angular/common';
import { MyApp } from './app.component';
import { AuthService } from '../shared/services/auth.service';
var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    NgModule({
        declarations: [
            MyApp
        ],
        imports: [
            OAuthModule.forRoot(),
            IonicModule.forRoot(MyApp, {}, {
                links: []
            }),
            SharedModule,
            LoginModule,
            HomeModule,
            DashboardModule,
            SettingsModule,
            DataGridModule
        ],
        bootstrap: [IonicApp],
        entryComponents: [
            MyApp
        ],
        providers: [
            AuthService,
            { provide: ErrorHandler, useClass: IonicErrorHandler },
            { provide: LocationStrategy, useClass: PathLocationStrategy },
            { provide: APP_BASE_HREF, useValue: '/' }
        ]
    })
], AppModule);
export { AppModule };
//# sourceMappingURL=app.module.js.map