import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { AppConstant } from '../../../../app.constants';

@Injectable()
export class UsersService implements Resolve<any>
{
    users: any[];
    onUsersChanged: BehaviorSubject<any>;

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
        this.onUsersChanged = new BehaviorSubject({});
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
                this.getUsers()
            ]).then(
                () => {
                    resolve();
                },
                reject
            );
        });
    }

    /**
     * Get users
     *
     * @returns {Promise<any>}
     */
    getUsers(): Promise<any>
    {
        return new Promise((resolve, reject) => {
            this.httpClient.get(`${this.appConstant.Server}api/users`)
                .subscribe((response: any) => {
                    this.users = response;
                    this.onUsersChanged.next(this.users);
                    resolve(response);
                }, reject);
        });
    }

     /**
     * Update users
     *
     * @param users
     * @returns {Promise<any>}
     */
    UpdateUsers(users): Promise<any>
    {
        return new Promise((resolve, reject) => {
            this.httpClient.post(`${this.appConstant.Server}api/users`, [...users])
            .subscribe((response: any) => {
                this.getUsers();
            }, reject);
        });
    }
}
