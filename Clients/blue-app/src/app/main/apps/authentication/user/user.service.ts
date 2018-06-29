import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable()
export class UserService implements Resolve<any>
{
    routeParams: any;
    user: any;
    onUserChanged: BehaviorSubject<any>;

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
        this.onUserChanged = new BehaviorSubject({});
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
                this.getUser()
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
    getUser(): Promise<any>
    {
        return new Promise((resolve, reject) => {
            if ( this.routeParams.id === 'new' )
            {
                this.onUserChanged.next(false);
                resolve(false);
            }
            else
            {
                this._httpClient.get('api/users/' + this.routeParams.id)
                    .subscribe((response: any) => {
                        this.user = response;
                        this.onUserChanged.next(this.user);
                        resolve(response);
                    }, reject);
            }
        });
    }

    /**
     * Save user
     *
     * @param user
     * @returns {Promise<any>}
     */
    saveUser(user): Promise<any>
    {
        return new Promise((resolve, reject) => {
            this._httpClient.post('api/users/' + user.id, user)
                .subscribe((response: any) => {
                    resolve(response);
                }, reject);
        });
    }

    /**
     * Add user
     *
     * @param user
     * @returns {Promise<any>}
     */
    addUser(user): Promise<any>
    {
        return new Promise((resolve, reject) => {
            this._httpClient.post('api/users/', user)
                .subscribe((response: any) => {
                    resolve(response);
                }, reject);
        });
    }

    /**
     * Delete user
     *
     * @param user
     * @returns {Promise<any>}
     */
    deleteUser(user): Promise<any>
    {
        return new Promise((resolve, reject) => {
            this._httpClient.delete('api/users/', user)
                .subscribe((response: any) => {
                    resolve(response);
                }, reject);
        });
    }
}
