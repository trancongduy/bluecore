import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { CdkTableModule } from '@angular/cdk/table';
import { MatButtonModule, MatChipsModule, MatFormFieldModule, MatIconModule, MatInputModule, MatPaginatorModule, MatRippleModule, MatSelectModule, MatSnackBarModule, MatSortModule, MatTableModule, MatTabsModule, MatToolbarModule, MatSlideToggleModule } from '@angular/material';

import { FuseSharedModule } from '@fuse/shared.module';
import { FuseWidgetModule } from '@fuse/components/widget/widget.module';
import { FuseConfirmDialogModule, FuseSidebarModule } from '@fuse/components';

import { UsersComponent } from 'app/main/apps/authentication/users/users.component';
import { UsersService } from 'app/main/apps/authentication/users/users.service';
import { UserComponent } from 'app/main/apps/authentication/user/user.component';
import { UserService } from 'app/main/apps/authentication/user/user.service';
import { UserFormDialogComponent } from 'app/main/apps/authentication/user-form/user-form.component';
import { RolesComponent } from 'app/main/apps/authentication/roles/roles.component';
import { RolesService } from 'app/main/apps/authentication/roles/roles.service';
import { RoleComponent } from 'app/main/apps/authentication/role/role.component';
import { RoleService } from 'app/main/apps/authentication/role/role.service';
import { RoleCreateFormDialogComponent } from 'app/main/apps/authentication/role-create/role-create-form.component';

const routes: Routes = [
    {
        path     : 'users',
        component: UsersComponent,
        resolve  : {
            data: UsersService
        }
    },
    {
        path     : 'users/:id',
        component: UserComponent,
        resolve  : {
            data: UserService
        }
    },
    {
        path     : 'roles',
        component: RolesComponent,
        resolve  : {
            data: RolesService
        }
    },
    {
        path     : 'roles/:id',
        component: RoleComponent,
        resolve  : {
            data: RoleService
        }
    }
];

@NgModule({
    declarations: [
        UsersComponent,
        UserComponent,
        UserFormDialogComponent,
        RolesComponent,
        RoleComponent,
        RoleCreateFormDialogComponent
    ],
    imports     : [
        RouterModule.forChild(routes),
        TranslateModule,

        CdkTableModule,
        MatButtonModule,
        MatChipsModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatPaginatorModule,
        MatRippleModule,
        MatSelectModule,
        MatSortModule,
        MatSnackBarModule,
        MatTableModule,
        MatTabsModule,
        MatToolbarModule,
        MatSlideToggleModule,

        FuseSharedModule,
        FuseWidgetModule,
        FuseConfirmDialogModule,
        FuseSidebarModule
    ],
    providers   : [
        UsersService,
        UserService,
        RolesService,
        RoleService
    ],
    entryComponents: [
        UserFormDialogComponent,
        RoleCreateFormDialogComponent
    ]
})
export class AuthenticationModule
{
}
