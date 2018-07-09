import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AccountRoutes } from './account.routing';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { AlertModule } from 'ngx-bootstrap/alert';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { ChangePasswordComponent } from './change-password/change-password.component';

export function HttpLoaderFactory(http: HttpClient) {
    return new TranslateHttpLoader(http);
}

@NgModule({
  imports: [
    CommonModule,
    AccountRoutes,
    FormsModule,
    ReactiveFormsModule,
    AlertModule.forRoot(),
    TranslateModule.forChild(),
  ],
  declarations: [LoginComponent]
})
export class AccountModule { }
