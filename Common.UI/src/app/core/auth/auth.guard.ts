import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs/Observable';
import { UserProfile } from 'app/core/models';


@Injectable()
export class AuthGuard implements CanActivate {

    constructor(private _authService: AuthService, private _router: Router) {
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {
        if (!this.isUserLogged()) {
            this._router.navigate(['account/login']);
        }

        return this.isUserAuthorized(route);
    }

    isUserLogged(): boolean {
        if (this._authService.isLogged) {
            const expirationDate = new Date(this._authService.currentUser.tokenExpirationDate);
            if (expirationDate.getDate() < new Date().getDate()) {
                this._authService.logout();
                return false;
            }
            return true;
        }
        return false;
    }

    isUserAuthorized(route: ActivatedRouteSnapshot): boolean {
        try {
            const user: UserProfile = this._authService.currentUser;
            if (!user) {
                return false;
            }
            return true;
        } catch (e) {
            sessionStorage.clear();
            return false;
        }
    }
}
