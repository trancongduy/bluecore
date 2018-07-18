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
    isActive: number=0;

    // Private
    private unsubscribeAll: Subject<any>;

    /**
     * Constructor
     *
     * @param {RoleService} roleService
     * @param {FormBuilder} formBuilder
     * @param {MatSnackBar} matSnackBar
     * @param {MatDialog} matDialog
     * @param {Router} router
     * @param {FuseTranslationLoaderService} fuseTranslationLoaderService
     * 
     */
    constructor(
        private roleService: RoleService,
        private formBuilder: FormBuilder,
        private matSnackBar: MatSnackBar,
        private matDialog: MatDialog,
        private router: Router,
        private fuseTranslationLoaderService : FuseTranslationLoaderService
    )
    {
        // Set the default
        this.role = new Role();

        // Set the private defaults
        this.unsubscribeAll = new Subject();
        this.fuseTranslationLoaderService.loadTranslations(english, vietnamese);
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
        this.roleService.onRoleChanged
            .pipe(takeUntil(this.unsubscribeAll))
            .subscribe(role => {

                if ( role )
                {
                    this.role = role;
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
        this.unsubscribeAll.next();
        this.unsubscribeAll.complete();
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
        return this.formBuilder.group({
            id              : [this.role.id],
            code            : [{value: this.role.code, disabled: true}, Validators.required],
            name            : [this.role.name, Validators.required],
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

        this.roleService.updateRole(data)
            .then(() => {

                // Trigger the subscription with new data
                this.roleService.onRoleChanged.next(data);

                // Show the success message
                this.matSnackBar.open('Your changed have been saved', 'OK', {
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
        this.confirmDialogRef = this.matDialog.open(FuseConfirmDialogComponent, {
            disableClose: false
        });

        this.confirmDialogRef.componentInstance.confirmMessage = 'Are you sure you want to delete?';

        this.confirmDialogRef.afterClosed().subscribe(result => {
            if ( result )
            {
                this.roleService.deleteRole(this.role);
                this.router.navigate(['/apps/authentication/roles']);
            }
            this.confirmDialogRef = null;
        });
    }
}