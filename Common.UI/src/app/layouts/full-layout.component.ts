import { AuthService } from './../core/auth/auth.service';
import { UserProfile } from 'app/core/models';
import { TranslateService } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import { CommonService } from 'app/shared/services';
import { MenuItem } from 'app/core/models/menu-item';
import { error } from 'util';
import { Spinkit } from 'ng-http-loader/spinkits';
import { routerTransition } from '../core/animations/router.animations';

@Component({
  selector: 'app-dashboard',
  templateUrl: './full-layout.component.html',
  styleUrls: ['./full-layout.component.css'],
  animations: [routerTransition],
})
export class FullLayoutComponent implements OnInit {

  public currentUser: UserProfile;
  public disabled = false;
  public status: { isopen: boolean } = { isopen: false };
  public menuItems: MenuItem[] = [];
  public loading = true;

  public spinkit = Spinkit;

  constructor(translate: TranslateService,
    private authService: AuthService,
    private _commonService: CommonService) {
    translate.use('es');
  }

  ngOnInit(): void {
    Promise.all([
      this.authService.validateToken(),
      this._commonService.getMenu()
    ]).then((data) => {
      this.currentUser = this.authService.currentUser;
      this.menuItems = this.authService.getMenuItems(data[1]);
      this.loading = false;
    }).catch(err => {
      this.authService.logout();
    });
  }

  public toggled(open: boolean): void {
    console.log('Dropdown is now: ', open);
  }

  public toggleDropdown($event: MouseEvent): void {
    $event.preventDefault();
    $event.stopPropagation();
    this.status.isopen = !this.status.isopen;
  }

  public logout() {
    this.authService.logout();
  }

  getState(outlet) {
    return outlet.activatedRouteData;
  }

}
