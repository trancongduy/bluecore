import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable()
export class RolesService implements Resolve<any>
{
    roles: any[];
    onRolesChanged: BehaviorSubject<any>;

    /**
     * Constructor
     *
     * @param {HttpClient} _httpClient
     */
    constructor(
        private _httpClient: HttpClient
    )
    {
        // Set the defaults
        this.onRolesChanged = new BehaviorSubject({});
    }

    /**
     * Resolver
     *
     * @param {ActivatedRouteSnapshot} route
     * @param {RouterStateSnapshot} state
     * @returns {Observable<any> | Promise<any> | any}
     */
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any
    {
        return new Promise((resolve, reject) => {

            Promise.all([
                this.getRoles()
            ]).then(
                () => {
                    resolve();
                },
                reject
            );
        });
    }

    /**
     * Get roles
     *
     * @returns {Promise<any>}
     */
    getRoles(): Promise<any>
    {
        return new Promise((resolve, reject) => {
            this._httpClient.get('api/roles')
                .subscribe((response: any) => {
                    this.roles = response;
                    this.onRolesChanged.next(this.roles);
                    resolve(response);
                }, reject);
        });
    }

     /**
     * Update roles
     *
     * @param roles
     * @returns {Promise<any>}
     */
    UpdateRoles(roles): Promise<any>
    {
        return new Promise((resolve, reject) => {
            this._httpClient.post('api/roles', [...roles])
            .subscribe((response: any) => {
                this.getRoles();
            }, reject);
        });
    }
}
