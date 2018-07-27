import { UserListComponent } from './users/user-list/user-list.component';
import { UserEditComponent } from './users/user-edit/user-edit.component';
import { Routes, RouterModule } from '@angular/router';
import { RoleEditComponent } from './roles/role-edit/role-edit.component';
import { RoleListComponent } from './roles/role-list/role-list.component';

const routes: Routes = [
    {
        path: 'users',
        data: {
            title: 'Users'
        },
        children: [
            { path: ':userName', component: UserEditComponent },
            { path: '', component: UserListComponent }
        ]
    },
    {
      path: 'roles',
      data: {
          title: 'Roles'
      },
      children: [
          { path: ':code', component: RoleEditComponent },
          { path: '', component: RoleListComponent }
      ]
  },
];

export const SettingsRoutes = RouterModule.forChild(routes);
