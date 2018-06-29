var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { NgModule } from '@angular/core';
import { IonicModule } from 'ionic-angular';
import { IonicStorageModule } from '@ionic/storage';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule, Http } from '@angular/http';
import { Deeplinks } from '@ionic-native/deeplinks';
import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { Network } from '@ionic-native/network';
import { TranslateModule, TranslateLoader, TranslateStaticLoader } from 'ng2-translate';
import { TruncatePipe } from './pipes/truncate.pipe';
import { TrimHtmlPipe } from './pipes/trim-html.pipe';
import { Config } from '../app.config';
export function createTranslateLoader(http) {
    return new TranslateStaticLoader(http, './assets/translations', '.json');
}
var SharedModule = (function () {
    function SharedModule() {
    }
    return SharedModule;
}());
SharedModule = __decorate([
    NgModule({
        declarations: [
            TruncatePipe,
            TrimHtmlPipe
        ],
        imports: [
            BrowserModule,
            IonicModule,
            IonicStorageModule.forRoot(),
            CommonModule,
            HttpModule,
            TranslateModule.forRoot({
                provide: TranslateLoader,
                useFactory: (createTranslateLoader),
                deps: [Http]
            })
        ],
        exports: [
            BrowserModule,
            HttpModule,
            IonicModule,
            TranslateModule,
            TruncatePipe,
            TrimHtmlPipe
        ],
        providers: [
            Deeplinks,
            StatusBar,
            SplashScreen,
            Network,
            Config
        ]
    })
], SharedModule);
export { SharedModule };
//# sourceMappingURL=shared.module.js.map