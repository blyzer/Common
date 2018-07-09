import { Component, OnInit, Injector } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BaseComponent } from '../../../core/components/base-component';
import { SUPER_EXPR } from '@angular/compiler/src/output/output_ast';
import { Subscription } from 'rxjs/Subscription';
import { Router, ActivatedRoute } from '@angular/router';
import { Route } from '@angular/compiler/src/core';
import { Role } from 'app/models';
import { CommonService, RoleService } from 'app/shared/services';
import { promise } from 'selenium-webdriver';
import { TranslateService } from '@ngx-translate/core';
import { FormControl } from '@angular/forms/src/model';

@Component({
  selector: 'app-role-edit',
  templateUrl: './role-edit.component.html',
  styleUrls: ['./role-edit.component.scss']
})
export class RoleEditComponent extends BaseComponent implements OnInit {

  loading = true;
  parameterSubscription: Subscription;
  title = 'EditRol';
  roleForm: FormGroup = new FormGroup({});
  isNew = true;
  availableAccess: string[];
  rol: Role;
  nameControl: FormControl;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder,
    private commonService: CommonService,
    private roleService: RoleService,
    private injector: Injector
  ) {

    super(injector);
  }

  ngOnInit() {
    this.parameterSubscription = this.route.params
      .subscribe(params => {
        const code = params['code'];
        this.initialize(code);
      });
  }

  initialize(code: string) {
    Promise.all([
      this.getRole(code),
      this.commonService.getAccessList(),
    ]).then(data => {
      ((role, accessList) => {
        this.rol = role;
        this.availableAccess = accessList.filter(a => !role.access.some(ra => ra === a));
        this.initializeForm(role);
        this.loading = false;

      }).apply(this, data);
    });
  }

  getRole(code: string): Promise<Role> {
    if (code === 'new') {
      this.title = 'AddRol';
      return Promise.resolve(new Role());
    }
    this.isNew = false;
    return this.roleService.get(code);
  }

  initializeForm(role: Role) {
    const roleNameValidators = [Validators.required];
    if (this.isNew) {
      roleNameValidators.push(this.roleService.checkDuplicatedModel.bind(this.roleService));
    }
    this.roleForm = this.formBuilder.group({
      nameDisabled: [{ value: role.name, disabled: true }],
      name: [role.name, roleNameValidators],
      active: [!!role.name ? role.active : true, Validators.required]
    });
    this.nameControl = this.roleForm.get('name') as FormControl;
  }

  onSubmit() {
    const rol: Role = this.roleForm.value;
    rol.access = this.rol.access;
    let defered: Promise<void> = null;
    if (this.isNew) {
      defered = this.roleService.add(rol);
    } else {
      defered = this.roleService.edit(rol);
    }

    Promise.all([
      defered,
      this.translateService.get('RolSavedMessage').toPromise()
    ]).then(data => {
      this.toastr.success(data[1]);
      if (this.isNew) {
        this.title = 'EditRol';
        this.router.navigate(['settings/roles/', rol.name]);
      }
    }).catch();
  }
}
