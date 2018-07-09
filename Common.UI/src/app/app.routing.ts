import { AuthGuard } from './core/auth/auth.guard';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// Layouts
import { FullLayoutComponent } from './layouts/full-layout.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { ChangePasswordComponent } from './account/change-password/change-password.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full',
  },
  {
    path: '',
    component: FullLayoutComponent,
    data: {
      title: 'HomeStart'
    },
    canActivate: [AuthGuard],
    children: [
      { path: 'dashboard', loadChildren: './dashboard/dashboard.module#DashboardModule' },
      { path: 'settings', loadChildren: './settings/settings.module#SettingsModule' },
      {
        path: 'account/change-password',
        component: ChangePasswordComponent,
        data: {
          title: 'ChangePassword'
        },
      }
    ]
  },
  { path: 'account', loadChildren: './account/account.module#AccountModule' },
  { path: '404', component: NotFoundComponent }
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
