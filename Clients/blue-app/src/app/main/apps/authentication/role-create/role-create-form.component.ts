import { Component, Inject, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { FuseTranslationLoaderService } from '@fuse/services/translation-loader.service';

import { locale as english } from './i18n/en';
import { locale as vietnamese } from './i18n/vi';

import { Role } from 'app/main/apps/authentication/role.model';

@Component({
    selector     : 'role-create-form-dialog',
    templateUrl  : './role-create-form.component.html',
    styleUrls    : ['./role-create-form.component.scss'],
    encapsulation: ViewEncapsulation.None
})

export class RoleCreateFormDialogComponent
{
    action: string;
    role: Role;
    roleForm: FormGroup;
    dialogTitle: string;
    addAnother: boolean;

    /**
     * Constructor
     *
     * @param {MatDialogRef<RoleCreateFormDialogComponent>} matDialogRef
     * @param _data
     * @param {FormBuilder} _formBuilder
     * @param {FuseTranslationLoaderService} _fuseTranslationLoaderService
     */
    constructor(
        public matDialogRef: MatDialogRef<RoleCreateFormDialogComponent>,
        @Inject(MAT_DIALOG_DATA) private _data: any,
        private _formBuilder: FormBuilder,
        private _fuseTranslationLoaderService: FuseTranslationLoaderService,
    )
    {
        // this.dialogTitle = 'NEW ROLE';
        this.role = new Role({});
        this.roleForm = this.createRoleForm();
        this.addAnother = false;

        this._fuseTranslationLoaderService.loadTranslations(english, vietnamese);
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
            code        : [this.role.code, Validators.required],
            name        : [this.role.name, Validators.required],
            description : [this.role.description]
        });
    }

    /**
     * Create role form
     *
     * @returns {void}
     */
    submitRole(roleForm: FormGroup) : void 
    {
        if(this.addAnother !== true) {
            this.matDialogRef.close(roleForm);
        }
        else {
            this.roleForm.reset();
        }
    }
}
