import { NgModule } from '@angular/core';
import { IonicModule } from 'ionic-angular';
import { IonicStorageModule } from '@ionic/storage';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule, Http } from '@angular/http';
import { Deeplinks } from '@ionic-native/deeplinks';
import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { SQLite } from '@ionic-native/sqlite';
import { Network } from '@ionic-native/network';

import { TranslateModule, TranslateLoader, TranslateStaticLoader } from 'ng2-translate';

import { TruncatePipe } from './pipes/truncate.pipe';
import { TrimHtmlPipe } from './pipes/trim-html.pipe';

import { Config } from '../app.config';

export function createTranslateLoader(http: Http) {
  return new TranslateStaticLoader(http, './assets/translations', '.json');
}

@NgModule({
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
    SQLite,
    Network,
    Config
  ]
})
export class SharedModule {}
