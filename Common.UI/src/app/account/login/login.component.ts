import { Component, OnInit, ViewEncapsulation, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthRequest } from 'app/core/models';
import { AuthService } from 'app/core/auth';
import { HttpStatus } from 'app/core/http';
import { BaseComponent } from 'app/core/components/base-component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent extends BaseComponent {

  public formLogin: FormGroup;
  public message: string;
  public loginButtonTitle = 'Login';

  constructor(private router: Router, private formBuilder: FormBuilder,
    private authService: AuthService,
    injector: Injector) {

    super(injector);
    this.createForm();
  }

  createForm() {
    const now = new Date();

    this.formLogin = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });

  }

  login() {
    if (!this.formLogin.valid) {
      return;
    }

    this.message = null;
    this.loginButtonTitle = 'PleaseWaitMessage';

    this.authService.login(this.formLogin.value as AuthRequest)
      .then(data => {
        this.router.navigate(['/']);
        this.loginButtonTitle = 'Login';
      }).catch(error => {
        this.loginButtonTitle = 'Login';
        console.log(error);
        if (error.status === HttpStatus.UNAUTHORIZED) {
          this.message = 'InvalidCredentials';
        } else {
          this.message = 'AnErrorHasOcurred';
        }
      });
  }

}
