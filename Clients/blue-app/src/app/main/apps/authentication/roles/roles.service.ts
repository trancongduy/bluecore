import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { AppConstant } from '../../../../app.constants';


@Injectable()
export class RolesService implements Resolve<any>
{
    roles: any[];
    onRolesChanged: BehaviorSubject<any>;

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
            this.httpClient.get(`${this.appConstant.Server}api/roles`)
                .subscribe((response: any) => {
                    this.roles = response;
                    this.onRolesChanged.next(this.roles);
                    resolve(response);
                }, reject);
        });
    }

     /**
     * Create roles
     *
     * @param roles
     * @returns {Promise<any>}
     */
    CreateRoles(roles): Promise<any>
    {
        return new Promise((resolve, reject) => {
            this.httpClient.post(`${this.appConstant.Server}api/rolecollections`, [...roles])
            .subscribe((response: any) => {
                this.getRoles();
            }, reject);
        });
    }
}
