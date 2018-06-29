import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar, MatDialog, MatDialogRef } from '@angular/material';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/internal/operators';

import { fuseAnimations } from '@fuse/animations';
import { FuseConfirmDialogComponent } from '@fuse/components/confirm-dialog/confirm-dialog.component';
import { FuseUtils } from '@fuse/utils';
import { FuseTranslationLoaderService } from '@fuse/services/translation-loader.service';

import { Role } from 'app/main/apps/authentication/role.model';
import { RoleService } from 'app/main/apps/authentication/role/role.service';

import { locale as english } from './i18n/en';
import { locale as vietnamese } from './i18n/vi';

@Component({
    selector   : 'role',
    templateUrl: './role.component.html',
    styleUrls  : ['./role.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations : fuseAnimations
})
export class RoleComponent implements OnInit, OnDestroy
{
    role: Role;
    pageType: string;
    roleForm: FormGroup;
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
        private _roleService: RoleService,
        private _formBuilder: FormBuilder,
        private _matSnackBar: MatSnackBar,
        private _matDialog: MatDialog,
        private _router: Router,
        private _fuseTranslationLoaderService : FuseTranslationLoaderService
    )
    {
        // Set the default
        this.role = new Role();

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
        // Subscribe to update role on changes
        this._roleService.onRoleChanged
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(role => {

                if ( role )
                {
                    this.role = new Role(role);
                    this.pageType = 'edit';
                }

                this.roleForm = this.createRoleForm();
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
     * Create role form
     *
     * @returns {FormGroup}
     */
    createRoleForm(): FormGroup
    {
        return this._formBuilder.group({
            id              : [this.role.id],
            code            : [this.role.code],
            name            : [this.role.name],
            description     : [this.role.description],
            active          : [this.role.active]
        });
    }

    /**
     * Save role
     */
    saveRole(): void
    {
        const data = this.roleForm.getRawValue();
        data.handle = FuseUtils.handleize(data.name);

        this._roleService.saveRole(data)
            .then(() => {

                // Trigger the subscription with new data
                this._roleService.onRoleChanged.next(data);

                // Show the success message
                this._matSnackBar.open('Your changed have been saved', 'OK', {
                    verticalPosition: 'top',
                    duration        : 2000
                });
            });
    }

    /**
     * Delete role
     */
    deleteRole(event): void
    {
        this.confirmDialogRef = this._matDialog.open(FuseConfirmDialogComponent, {
            disableClose: false
        });

        this.confirmDialogRef.componentInstance.confirmMessage = 'Are you sure you want to delete?';

        this.confirmDialogRef.afterClosed().subscribe(result => {
            if ( result )
            {
                this._roleService.deleteRole(this.role);
                this._router.navigate(['/apps/authentication/roles']);
            }
            this.confirmDialogRef = null;
        });
    }
}