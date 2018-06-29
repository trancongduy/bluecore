import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar, MatDialog, MatDialogRef } from '@angular/material';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/internal/operators';

import { fuseAnimations } from '@fuse/animations';
import { FuseConfirmDialogComponent } from '@fuse/components/confirm-dialog/confirm-dialog.component';
import { FuseUtils } from '@fuse/utils';
import { FuseTranslationLoaderService } from '@fuse/services/translation-loader.service';

import { User } from 'app/main/apps/authentication/user.model';
import { UserService } from 'app/main/apps/authentication/user/user.service';

import { locale as english } from './i18n/en';
import { locale as vietnamese } from './i18n/vi';

@Component({
    selector   : 'user',
    templateUrl: './user.component.html',
    styleUrls  : ['./user.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations : fuseAnimations
})
export class UserComponent implements OnInit, OnDestroy
{
    user: User;
    pageType: string;
    userForm: FormGroup;
    confirmDialogRef: MatDialogRef<FuseConfirmDialogComponent>;

    // Private
    private _unsubscribeAll: Subject<any>;

    /**
     * Constructor
     *
     * @param {RoleService} _roleService
     * @param {FormBuilder} _formBuilder
     * @param {MatSnackBar} _matSnackBar
     * @param {MatDialog} _matDialog
     * @param {Router} _router
     * @param {FuseTranslationLoaderService} _fuseTranslationLoaderService
     * 
     */
    constructor(
        private _userService: UserService,
        private _formBuilder: FormBuilder,
        private _matSnackBar: MatSnackBar,
        private _matDialog: MatDialog,
        private _router: Router,
        private _fuseTranslationLoaderService : FuseTranslationLoaderService
    )
    {
        // Set the default
        this.user = new User();

        // Set the private defaults
        this._unsubscribeAll = new Subject();
        this._fuseTranslationLoaderService.loadTranslations(english, vietnamese);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void
    {
        // Subscribe to update user on changes
        this._userService.onUserChanged
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(user => {

                if ( user )
                {
                    this.user = new User(user);
                    this.pageType = 'edit';
                }

                this.userForm = this.createUserForm();
            });
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void
    {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Create user form
     *
     * @returns {FormGroup}
     */
    createUserForm(): FormGroup
    {
        return this._formBuilder.group({
            id              : [this.user.id],
            name            : [this.user.name],
            email           : [''],  
            firstName       : ['', Validators.required],
            lastName        : ['', Validators.required],
            description     : [this.user.description],
            active          : [this.user.active]
        });
    }

    /**
     * Save user
     */
    saveUser(): void
    {
        const data = this.userForm.getRawValue();
        data.handle = FuseUtils.handleize(data.name);

        this._userService.saveUser(data)
            .then(() => {

                // Trigger the subscription with new data
                this._userService.onUserChanged.next(data);

                // Show the success message
                this._matSnackBar.open('Your changed have been saved', 'OK', {
                    verticalPosition: 'top',
                    duration        : 2000
                });
            });
    }

    /**
     * Delete user
     */
    deleteUser(event): void
    {
        this.confirmDialogRef = this._matDialog.open(FuseConfirmDialogComponent, {
            disableClose: false
        });

        this.confirmDialogRef.componentInstance.confirmMessage = 'Are you sure you want to delete?';

        this.confirmDialogRef.afterClosed().subscribe(result => {
            if ( result )
            {
                this._userService.deleteUser(this.user);
                this._router.navigate(['/apps/authentication/users']);
            }
            this.confirmDialogRef = null;
        });
    }
}