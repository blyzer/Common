import { LoginComponent } from './login/login.component';
import { Routes, RouterModule } from '@angular/router';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { AuthGuard } from '../core/auth';
import { FullLayoutComponent } from '../layouts/full-layout.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
];

export const AccountRoutes = RouterModule.forChild(routes);
