import { NgModule } from '@angular/core';

import { HomeModule } from './home/home.module';
import { LoginModule } from 'app/main/pages/authentication/login/login.module';
import { Error404Module } from 'app/main/pages/errors/404/error-404.module';
import { Error500Module } from 'app/main/pages/errors/500/error-500.module';

@NgModule({
    imports: [
        HomeModule,

        // Authentication
        LoginModule,

        // Errors
        Error404Module,
        Error500Module
    ]
})
export class PagesModule
{

}
