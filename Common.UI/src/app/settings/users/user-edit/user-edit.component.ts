import { Role, User } from 'app/models';
import { Component, OnInit, Injector } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UserService, RoleService, CommonService } from 'app/shared/services';
import { BaseComponent } from 'app/core/components/base-component';
import { UserProfile } from 'app/core/models';
import { AuthService } from '../../../core/auth/auth.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css'],
})
export class UserEditComponent extends BaseComponent implements OnInit {
  public isNew: boolean;
  public roles: Role[];
  public user: User;
  public userForm: FormGroup;
  public loading = true;
  public title: string;
  public showUserType = false;
  public currentUser: UserProfile;
  public userTypesList: string[];

  public get userType() {
    return this.userForm.get('userType');
  }
  public get email() {
    return this.userForm.get('email');
  }
  public get usernameControl() {
    return this.userForm.get('userName');
  }

  constructor(
    private _route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder,
    private usersService: UserService,
    private roleService: RoleService,
    authService: AuthService,
    private _commonService: CommonService,
    injector: Injector
  ) {
    super(injector);
    this.currentUser = authService.currentUser;
  }

  ngOnInit() {
    this.parameterSubscription = this._route.params.subscribe(params => {
      const userName = params['userName'];
      this.initialize(userName).then(() => {
        if (this.isNew) {
          const self = this;
          setTimeout(() => {
            self.userForm.controls['userName'].enable();
          }, 300);
        }
      });
    });
  }

  private initialize(username: string): Promise<void> {
    if (username === this.currentUser.userName) {
      history.replaceState(null, 'not-found', '404');
      this.router.navigate(['404']);
    }

    this.title = username === 'new' ? 'NewUser' : 'UserEdit';
    return Promise.all([this.getUser(username), this.roleService.getAll(), this._commonService.getUserTypesList()])
      .then(data => {
        ((user, roles, userTypes) => {
          this.user = user;
          this.roles = roles;
          console.log(this.user);
          this.showUserType = !this.currentUser.userType || this.currentUser.userType !== '*';
          this.userTypesList = userTypes;
          this.initializeForm(user);
          this.loading = false;
        }).apply(this, data);
      })
      .catch(this.handleHttpError);
  }

  private getUser(userName): Promise<User> {
    if (userName === 'new') {
      this.isNew = true;
      const user = new User();
      user.active = true;
      user.userType = 0;
      return Promise.resolve(user);
    }
    this.isNew = false;
    return this.usersService.get(userName);
  }

  private initializeForm(user: User): void {
    const passwordValidations = this.isNew ? [Validators.required] : [];
    const usernameValidators = [Validators.required];
    if (this.isNew) {
      usernameValidators.push(this.usersService.checkDuplicatedModel.bind(this.roleService));
    }

    this.userForm = this.formBuilder.group({
      userName: [user.userName, usernameValidators],
      userNameDisabled: [{ value: user.userName, disabled: true }],
      password: [user.password, Validators.compose(passwordValidations)],
      email: [user.email, Validators.compose([Validators.required, Validators.email])],
      firstName: [user.firstName, Validators.required],
      lastName: [user.lastName, Validators.required],
      active: [!!user.firstName ? user.active : true, Validators.required],
      roleName: [user.roleName || null, Validators.required],
      userType: [user.userType, Validators.required],
    });
    if (!!this.currentUser.userType) {
      this.setUserType(this.currentUser.userType);
    }
  }

  setUserType(userType: string) {
    this.userForm.patchValue({
      userType: userType,
    });
  }

  onRolChange() {
    const isSysAdmin = (this.userForm.value as User).roleName === 'SystemAdministrator';

    if (isSysAdmin) {
      this.showUserType = false;
      return;
    }

    if (!this.currentUser.userType) {
      this.showUserType = true;
    } else {
      this.showUserType = false;
      this.setUserType(this.currentUser.userType);
    }
  }

  onSubmit() {
    const self = this;
    let promise: Promise<void>;

    if (self.isNew) {
      promise = self.usersService.add(self.userForm.value as User);
    } else {
      promise = self.usersService.edit(self.user.userName, self.userForm.value as User);
    }

    promise
      .then(() => {
        self.translateService
          .get('UserSavedMessage')
          .toPromise()
          .then(message => {
            self.toastr.success(message);
            if (self.isNew) {
              self.router.navigate(['settings/users', self.userForm.value.userName]);
            }
          });
      })
      .catch(error => self.handleHttpError(error, self.toastr));
  }
}
