import { UserProfile } from './../models/user-profile';
import { AuthService } from './../auth/auth.service';
import { Injectable } from '@angular/core';
import 'rxjs/add/operator/toPromise';
import { Router } from '@angular/router';
import { HttpStatus } from './http-status-codes';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class BaseHttpService {
    constructor(private _http: HttpClient,
        private _router: Router,
        private _authService: AuthService) {
    }

    private addAuthorizationHeaders(options: any) {
        if (!options) {
            options = {};
        }

        if (!options.headers) {
            options.headers = new Headers();
        }
        const user: UserProfile = this._authService.currentUser;
        options.headers.append('Authorization', 'Bearer ' + user.token);

        return options;
    }

    get(url, options?: any): Promise<any> {
        options = this.addAuthorizationHeaders(options);
        return this._http.get(url, options)
            .toPromise()
            .catch(error => this.handleErrorAsPromise(error, this._authService));
    }

    post(url, data, options?): Promise<any> {
        options = this.addAuthorizationHeaders(options);
        return this._http.post(url, data, options)
            .toPromise()
            .catch(error => this.handleErrorAsPromise(error, this._authService));
    }

    put(url, data, options?): Promise<any> {
        options = this.addAuthorizationHeaders(options);
        return this._http.put(url, data, options)
            .toPromise()
            .catch(error => this.handleErrorAsPromise(error, this._authService));
    }

    protected handleErrorAsPromise(error: Response, authService: AuthService) {
        if (error.status !== HttpStatus.NOT_FOUND && error.status !== HttpStatus.UNAUTHORIZED) {
            console.error(error);
        }

        if (error.status === HttpStatus.UNAUTHORIZED) {
            // this._authService.logout();
            this._router.navigate(['/login']);
        }
        return Promise.reject(error || 'Server error');
    }
}
