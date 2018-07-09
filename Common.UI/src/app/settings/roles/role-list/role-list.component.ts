import { Component, OnInit } from '@angular/core';
import { CollumnDefinition } from 'app/core/data';
import { Role } from 'app/models';
import { RoleService } from 'app/shared/services';

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.scss']
})
export class RoleListComponent implements OnInit {

  public columns: CollumnDefinition[] = [];
  public roles: Role[] = [];
  constructor(private roleService: RoleService) { }

  ngOnInit() {
    this.roleService.getAll()
    .then(roles => {
      this.roles = roles;
      this.initializeTable();
    });
  }

  initializeTable() {
    const self = this;
    this.columns = [
      { id: 'roleName', field: 'name', header: 'Role' },
      { id: 'Active', field: 'active', header: 'Active' },
    ];
  }
}
