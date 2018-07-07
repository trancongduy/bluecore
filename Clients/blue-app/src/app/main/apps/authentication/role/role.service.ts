import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { AppConstant } from '../../../../app.constants';

@Injectable()
export class RoleService implements Resolve<any>
{
    routeParams: any;
    role: any;
    onRoleChanged: BehaviorSubject<any>;

    /**
     * Constructor
     *
     * @param {HttpClient} httpClient
     */
    constructor(
        private httpClient: HttpClient,
        private appConstant: AppConstant
    )
    {
        // Set the defaults
        this.onRoleChanged = new BehaviorSubject({});
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
        this.routeParams = route.params;

        return new Promise((resolve, reject) => {

            Promise.all([
                this.getRole()
            ]).then(
                () => {
                    resolve();
                },
                reject
            );
        });
    }

    /**
     * Get role
     *
     * @returns {Promise<any>}
     */
    getRole(): Promise<any>
    {
        return new Promise((resolve, reject) => {
            if ( this.routeParams.id === 'new' )
            {
                this.onRoleChanged.next(false);
                resolve(false);
            }
            else
            {
                this.httpClient.get(`${this.appConstant.Server}api/roles/` + this.routeParams.id)
                    .subscribe((response: any) => {
                        this.role = response;
                        this.onRoleChanged.next(this.role);
                        resolve(response);
                    }, reject);
            }
        });
    }

    /**
     * Update role
     *
     * @param role
     * @returns {Promise<any>}
     */
    updateRole(role): Promise<any>
    {
        return new Promise((resolve, reject) => {
            this.httpClient.put(`${this.appConstant.Server}api/roles/` + role.id, role)
                .subscribe((response: any) => {
                    resolve(response);
                }, reject);
        });
    }

    /**
     * Add role
     *
     * @param role
     * @returns {Promise<any>}
     */
    addRole(role): Promise<any>
    {
        return new Promise((resolve, reject) => {
            this.httpClient.post(`${this.appConstant.Server}api/roles/`, role)
                .subscribe((response: any) => {
                    resolve(response);
                }, reject);
        });
    }

    /**
     * Delete role
     *
     * @param role
     * @returns {Promise<any>}
     */
    deleteRole(role): Promise<any>
    {
        return new Promise((resolve, reject) => {
            this.httpClient.delete(`${this.appConstant.Server}api/roles/`, role)
                .subscribe((response: any) => {
                    resolve(response);
                }, reject);
        });
    }
}
