import { User } from 'app/models';
import { Router } from '@angular/router';
import { Component, OnInit, Injector } from '@angular/core';
import { CollumnType, CollumnDefinition } from 'app/core/data';
import { UserService } from 'app/shared/services';
import { BaseComponent } from 'app/core/components/base-component';
import { ToastsManager } from 'ng2-toastr';
import { AuthService } from 'app/core/auth';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent extends BaseComponent implements OnInit {
  public users: User[] = [];
  public columns: CollumnDefinition[];

  constructor(private router: Router,
    private userService: UserService,
    private authService: AuthService,
    injector: Injector) {

    super(injector);
  }

  ngOnInit() {
    this.fetchUsers(25, 0);
    this.initializeTable();
  }

  fetchUsers(count: number, pageNumber: number): void {
    this.userService.getAll(count, pageNumber)
      .then(users => {
        const currentUser = this.authService.currentUser;
        this.users = users.filter(u => u.userName !== currentUser.userName);
      })
      .catch(this.handleHttpError);
  }

  initializeTable() {
    const self = this;
    this.columns = [
      { id: 'userName', field: 'userName', header: 'UserName', type: CollumnType.Data },
      { id: 'email', field: 'email', header: 'Email', type: CollumnType.Data },
      { id: 'firstName', field: 'firstName', header: 'FirstName', type: CollumnType.Data },
      { id: 'lastName', field: 'lastName', header: 'LastName', type: CollumnType.Data },
      { id: 'roleName', field: 'roleName', header: 'Role', type: CollumnType.Data },
      { id: 'active', field: 'active', header: 'Active', type: CollumnType.Data },
    ];
  }

  delete(user: User) {
    console.log(`deleting useser ${user.userName}`);
  }

  edit(user: User) {
    this.router.navigate([`settings/users/${user.userName}`]);
  }

  isData(col: CollumnDefinition): boolean {
    return col.type === CollumnType.Data;
  }
}
