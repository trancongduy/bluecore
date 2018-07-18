import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

import { FuseTranslationLoaderService } from '@fuse/services/translation-loader.service';

import { locale as english } from './i18n/en';
import { locale as vietnamese } from './i18n/vi';

import { SecurityService } from 'app/shared/services/security.service';

@Component({
    selector   : 'home',
    templateUrl: './home.component.html',
    styleUrls  : ['./home.component.scss']
})
export class HomeComponent implements OnInit
{
    authenticated: boolean = false;
    authSubscription: Subscription;

    /**
     * Constructor
     *
     * @param {FuseTranslationLoaderService} _fuseTranslationLoaderService
     */
    constructor(
        private _fuseTranslationLoaderService: FuseTranslationLoaderService,
        private securityService: SecurityService
    )
    {
        this._fuseTranslationLoaderService.loadTranslations(english, vietnamese);
        this.authenticated = securityService.IsAuthorized;
    }

    ngOnInit() {

        // Subscribe to login and logout observable
        this.authSubscription = this.securityService.authenticationChallenge$.subscribe(res => {
            this.authenticated = res;
        });
    }
}
