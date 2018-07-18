import { Component, ElementRef, TemplateRef, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatPaginator, MatSort, MatDialog, MatDialogRef } from '@angular/material';
import { DataSource } from '@angular/cdk/collections';
import { merge, Observable, BehaviorSubject, fromEvent, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { takeUntil } from 'rxjs/internal/operators';

import { fuseAnimations } from '@fuse/animations';
import { FuseConfirmDialogComponent } from '@fuse/components/confirm-dialog/confirm-dialog.component';
import { FuseUtils } from '@fuse/utils';

import { FuseTranslationLoaderService } from '@fuse/services/translation-loader.service';

import { locale as english } from './i18n/en';
import { locale as vietnamese } from './i18n/vi';

import { RolesService } from 'app/main/apps/authentication/roles/roles.service';
import { RoleCreateFormDialogComponent } from 'app/main/apps/authentication/role-create/role-create-form.component';
import { Role } from 'app/main/apps/authentication/role.model';

@Component({
    selector   : 'roles',
    templateUrl: './roles.component.html',
    styleUrls  : ['./roles.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations : fuseAnimations
})
export class RolesComponent
{
    dataSource: FilesDataSource | null;
    displayedColumns = ['code', 'name', 'description', 'active'];
    dialogRef: any;
    confirmDialogRef: MatDialogRef<FuseConfirmDialogComponent>;
    roles: Role[];
    refresh: Subject<any> = new Subject();

    @ViewChild('dialogContent')
    dialogContent: TemplateRef<any>;

    @ViewChild(MatPaginator)
    paginator: MatPaginator;

    @ViewChild(MatSort)
    sort: MatSort;

    @ViewChild('filter')
    filter: ElementRef;

    // Private
    private unsubscribeAll: Subject<any>;

    /**
     * Constructor
     *
     * @param {FuseTranslationLoaderService} fuseTranslationLoaderService
     * @param {MatDialog} matDialog
     * @param {RolesService} rolesService
     * 
     */
    constructor(
        private fuseTranslationLoaderService: FuseTranslationLoaderService,
        private matDialog: MatDialog,
        private rolesService: RolesService
    )
    {
        // Set the private defaults
        this.unsubscribeAll = new Subject();
        this.fuseTranslationLoaderService.loadTranslations(english, vietnamese);

        /**
         * Get roles from service/server
         */
        this.setRoles();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void
    {
        /**
         * Watch re-render-refresh for updating db
         */
        this.refresh.subscribe(updateDB => {
            if ( updateDB )
            {
                this.rolesService.CreateRoles(this.roles.filter(x => x.id === ''));
            }
        });

        this.dataSource = new FilesDataSource(this.rolesService, this.paginator, this.sort);

        fromEvent(this.filter.nativeElement, 'keyup')
            .pipe(
                takeUntil(this.unsubscribeAll),
                debounceTime(150),
                distinctUntilChanged()
            )
            .subscribe(() => {
                if ( !this.dataSource )
                {
                    return;
                }

                this.dataSource.filter = this.filter.nativeElement.value;
            });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Set roles
     */
    setRoles(): void
    {
        this.roles = this.rolesService.roles.map(item => {
            return new Role(item);
        });
    }

    /**
     * Create role
     *
     * @param role
     */
    createRole(role): void
    {
        this.dialogRef = this.matDialog.open(RoleCreateFormDialogComponent, {
            panelClass: 'role-create-form-dialog',
            data      : {
                action : 'create'
            }
        });

        this.dialogRef.afterClosed()
            .subscribe(response => {
                if ( !response )
                {
                    return;
                }

                const newRole = response.getRawValue();
                this.roles.push(new Role({
                    code        : newRole.code,
                    name        : newRole.name,
                    description : newRole.description
                }));
                this.refresh.next(true);
            });
    }
}

export class FilesDataSource extends DataSource<any>
{
    private filterChange = new BehaviorSubject('');
    private filteredDataChange = new BehaviorSubject('');

    /**
     * Constructor
     *
     * @param {RolesService} rolesService
     * @param {MatPaginator} matPaginator
     * @param {MatSort} matSort
     */
    constructor(
        private rolesService: RolesService,
        private matPaginator: MatPaginator,
        private matSort: MatSort
    )
    {
        super();

        this.filteredData = this.rolesService.roles;
    }

    /**
     * Connect function called by the table to retrieve one stream containing the data to render.
     *
     * @returns {Observable<any[]>}
     */
    connect(): Observable<any[]>
    {
        const displayDataChanges = [
            this.rolesService.onRolesChanged,
            this.matPaginator.page,
            this.filterChange,
            this.matSort.sortChange
        ];

        return merge(...displayDataChanges)
            .pipe(
                map(() => {
                        let data = this.rolesService.roles.slice();

                        data = this.filterData(data);

                        this.filteredData = [...data];

                        data = this.sortData(data);

                        // Grab the page's slice of data.
                        const startIndex = this.matPaginator.pageIndex * this.matPaginator.pageSize;
                        return data.splice(startIndex, this.matPaginator.pageSize);
                    }
                ));
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    // Filtered data
    get filteredData(): any
    {
        return this.filteredDataChange.value;
    }

    set filteredData(value: any)
    {
        this.filteredDataChange.next(value);
    }

    // Filter
    get filter(): string
    {
        return this.filterChange.value;
    }

    set filter(filter: string)
    {
        this.filterChange.next(filter);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Filter data
     *
     * @param data
     * @returns {any}
     */
    filterData(data): any
    {
        if ( !this.filter )
        {
            return data;
        }
        return FuseUtils.filterArrayByString(data, this.filter);
    }

    /**
     * Sort data
     *
     * @param data
     * @returns {any[]}
     */
    sortData(data): any[]
    {
        if ( !this.matSort.active || this.matSort.direction === '' )
        {
            return data;
        }

        return data.sort((a, b) => {
            let propertyA: number | string = '';
            let propertyB: number | string = '';

            switch ( this.matSort.active )
            {
                case 'code':
                    [propertyA, propertyB] = [a.code, b.code];
                    break;
                case 'name':
                    [propertyA, propertyB] = [a.name, b.name];
                    break;
                case 'description':
                    [propertyA, propertyB] = [a.description, b.description];
                    break;
            }

            const valueA = isNaN(+propertyA) ? propertyA : +propertyA;
            const valueB = isNaN(+propertyB) ? propertyB : +propertyB;

            return (valueA < valueB ? -1 : 1) * (this.matSort.direction === 'asc' ? 1 : -1);
        });
    }

    /**
     * Disconnect
     */
    disconnect(): void
    {
    }
}