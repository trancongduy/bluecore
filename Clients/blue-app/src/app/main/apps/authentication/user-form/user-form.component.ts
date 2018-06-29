import { Component, Inject, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { User } from 'app/main/apps/authentication/user.model';

@Component({
    selector     : 'user-form-dialog',
    templateUrl  : './user-form.component.html',
    styleUrls    : ['./user-form.component.scss'],
    encapsulation: ViewEncapsulation.None
})

export class UserFormDialogComponent
{
    action: string;
    user: User;
    userForm: FormGroup;
    dialogTitle: string;
    addAnother: boolean;

    /**
     * Constructor
     *
     * @param {MatDialogRef<UserFormDialogComponent>} matDialogRef
     * @param _data
     * @param {FormBuilder} _formBuilder
     */
    constructor(
        public matDialogRef: MatDialogRef<UserFormDialogComponent>,
        @Inject(MAT_DIALOG_DATA) private _data: any,
        private _formBuilder: FormBuilder
    )
    {
        this.dialogTitle = 'New User';
        this.user = new User({});
        this.userForm = this.createUserForm();
        this.addAnother = true;
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
            id          : [this.user.id],
            name        : [this.user.name],
            description : [this.user.description]
        });
    }

    /**
     * Create user form
     *
     * @returns {void}
     */
    submitUser(userForm: FormGroup) : void 
    {
        if(this.addAnother !== true) {
            this.matDialogRef.close(userForm);
        }
        else {
            this.userForm.reset();
        }
    }
}
