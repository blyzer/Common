import { HttpStatus } from './http-status-codes';
import { HttpErrorResponse } from '@angular/common/http/src/response';
import { FormControl } from '@angular/forms';
import { BaseHttpService } from 'app/core/http';

export abstract class ModelService {
  private debouncher;

  constructor(protected http: BaseHttpService, protected baseUri: string) {
  }

  protected handleErrorAsPromise(error: HttpErrorResponse) {
    if (error.status !== HttpStatus.NOT_FOUND && error.status !== HttpStatus.UNAUTHORIZED) {
      console.error(error);
    }

    return Promise.reject(error || 'Server error');
  }

  checkDuplicatedModel(control: FormControl) {

    if (!!this.debouncher) {
      clearTimeout(this.debouncher);
    }

    this.debouncher = setTimeout(() => {
      if (!control.value) {
        if (!!control.errors) {
          delete control.errors.duplicated;
        }
        return;
      }

      return this.http.get(`${this.baseUri}/${control.value}/code-validation`)
        .then((data: any) => {
          if (!!data.duplicated) {
            control.setErrors({ duplicated: true });
          } else {
            if (!!control.errors) {
              delete control.errors.duplicated;
            }
          }
        });
    }, 1000);
  }
}
