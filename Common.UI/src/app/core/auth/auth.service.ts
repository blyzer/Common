import { AuthRequest } from './../models/auth-request';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import 'rxjs/add/operator/toPromise';
import { UserProfile } from 'app/core/models';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { MenuItem } from '../models/menu-item';
import { debug } from 'util';

@Injectable()
export class AuthService {
  private uri = 'api/accounts';

  private static get TOKEN_KEY(): string {
    return 'accessToken';
  }

  constructor(private http: HttpClient,
    private router: Router) {
  }

  public get isLogged(): boolean {
    return !!this.currentUser;
  }

  public static get currentUser(): UserProfile {
    return JSON.parse(sessionStorage.getItem(AuthService.TOKEN_KEY)) as UserProfile;
  }

  public get currentUser(): UserProfile {
    return JSON.parse(sessionStorage.getItem(AuthService.TOKEN_KEY)) as UserProfile;
  }

  logout(): Promise<void> {

    const user: UserProfile = this.currentUser;
    if (user === null) {
      return;
    }

    const options = {
      headers: new HttpHeaders()
    };
    options.headers.append('Authorization', 'Bearer ' + user.token);

    sessionStorage.removeItem(AuthService.TOKEN_KEY);
    return this.http.post(`${this.uri}/logout`, user, options)
      .toPromise()
      .then(resp => {
        this.router.navigate(['account/login']);
      });
  }

  login(authRequest: AuthRequest): Promise<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const options = { headers: headers };

    return this.http.post(`${this.uri}/login`, authRequest, options)
      .toPromise()
      .then(resp => {
        sessionStorage.setItem(AuthService.TOKEN_KEY, JSON.stringify(resp));
        return resp;
      });
  }

  validateToken(): Promise<void> {

    const options: any = {};

    if (!options.headers) {
      options.headers = new HttpHeaders();
    }

    options.headers.append('Authorization', 'Bearer ' + this.currentUser.token);

    return this.http.get(`${this.uri}/token-validation-request`, options)
      .toPromise()
      .then(resp => { return; });
  }

  getMenuItems(menus: MenuItem[]) {
    const userAccess: string[] = this.currentUser.accessList;

    menus = menus.filter(menu => {
      if ((menu.subMenus || []).length > 0) {
        menu.subMenus = menu.subMenus.filter(sub => userAccess.some(access => access.toLocaleLowerCase() === sub.key));
      }
      return menu.subMenus.length > 0;
    });

    return menus;
  }

  changePassword(currentPassword: string, newPassword: string): Promise<void> {
    const formData: FormData = new FormData();
    formData.append('currentPassword', currentPassword);
    formData.append('newPassword', newPassword);

    return this.http.put(`${this.uri}/password`, formData).toPromise()
      .then(() => { });
  }
}
