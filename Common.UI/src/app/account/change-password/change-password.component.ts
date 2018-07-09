import { Component, OnInit, Injector } from '@angular/core';
import { AuthService } from 'app/core/auth';
import { BaseComponent } from 'app/core/components/base-component';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FormControl } from '@angular/forms/src/model';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent extends BaseComponent implements OnInit {

  form: FormGroup;
  loading = true;
  currentPassword: FormControl;
  newPassword: FormControl;
  confirmPassword: FormControl;

  constructor(private authService: AuthService, private fb: FormBuilder, injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.form = this.fb.group({
      currentPassword: ['', Validators.required],
      newPassword: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    }, { validator: this.checkIfMatchingPasswords('newPassword', 'confirmPassword') });

    this.currentPassword = this.form.get('currentPassword') as FormControl;
    this.newPassword = this.form.get('newPassword') as FormControl;
    this.confirmPassword = this.form.get('confirmPassword') as FormControl;
    this.loading = false;
  }

  checkIfMatchingPasswords(passwordKey: string, passwordConfirmationKey: string) {
    return (group: FormGroup) => {
      const passwordInput = group.controls[passwordKey],
        passwordConfirmationInput = group.controls[passwordConfirmationKey];

      if (passwordInput.value !== passwordConfirmationInput.value) {
        passwordConfirmationInput.setErrors({ notMach: true });
        passwordConfirmationInput.markAsTouched();
      }
    };
  }

  onSubmit() {
    if (!this.form.valid) {
      return;
    }

    const value: any = this.form.value;
    Promise.all([
      this.authService.changePassword(value.currentPassword, value.newPassword),
      this.translateService.get('PasswordChangedMessage').toPromise()
    ]).then((data) => {
      this.toastr.success(data[1]);
    });
  }
}
