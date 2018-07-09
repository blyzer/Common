import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ToastsManager } from 'ng2-toastr';
import { IMyDpOptions } from 'mydatepicker';
import { debug } from 'util';
import { ContactType, ContactInformation } from 'app/models';
import { FormArray } from '@angular/forms/src/model';
import { TranslateService } from '@ngx-translate/core';
import { Injector, ReflectiveInjector, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
export class BaseComponent implements OnDestroy {
  parameterSubscription: Subscription;

  public myDatePickerOptions: IMyDpOptions = {
    // other options...
    dateFormat: 'dd/mm/yyyy',
  };

  public myTimePickerOptions: IMyDpOptions = {
    // other options...
    dateFormat: 'dd/mm/yyyy',
  };

  translateService: TranslateService;
  toastr: ToastsManager;

  constructor(injector: Injector) {
    this.translateService = injector.get(TranslateService);
    this.toastr = injector.get(ToastsManager);
  }

  protected handleHttpError(error: any, toastr?: ToastsManager): void {
    try {
      const body = JSON.parse(error.text()) as any;
      let message = '';
      if (body.length) {
        message = (body as any[]).join('\n');
      } else if (body.messsage) {
        message = message;
      } else {
        message = body;
      }

      if (toastr) {
        toastr.error(message);
      }
    } catch (error) {
      console.log('ops! somthing go wrong on handleHttpError');
    }
  }

  hasErrors(form: FormGroup, controlName: string): boolean {
    return (
      form.controls[controlName].invalid && (form.controls[controlName].dirty || form.controls[controlName].touched)
    );
  }

  getContactInputType(controlType) {
    if (controlType.value === ContactType[ContactType.Email]) {
      return 'email';
    }

    return 'tel';
  }

  getContactInputPlaceHolder(controlType) {
    if (controlType.value === ContactType[ContactType.Email]) {
      return 'email@mail.com';
    }

    return '809 5555 5555';
  }

  addNewContactInformation(contactInformation: FormArray, formBuilder: FormBuilder) {
    contactInformation.push(this.createContactInformationGroup(new ContactInformation(), formBuilder));
  }

  removeContactInformationElement(contactInformation: FormArray, element: number) {
    contactInformation.removeAt(element);
  }

  createContactInformationArray(contacts: ContactInformation[], formBuilder: FormBuilder): FormArray {
    const contactInformation = formBuilder.array([]);

    contacts.forEach(contact => {
      contactInformation.push(this.createContactInformationGroup(contact, formBuilder));
    });

    return contactInformation;
  }

  createContactInformationGroup(contact: ContactInformation, formBuilder: FormBuilder): FormGroup {
    return formBuilder.group({
      contactType: [contact.contactType, Validators.required],
      contactValue: [contact.contactValue, Validators.required],
    });
  }

  ngOnDestroy(): void {
    if (this.parameterSubscription) {
      this.parameterSubscription.unsubscribe();
    }
  }

  syncSelectors(current: HTMLSelectElement, target: HTMLSelectElement) {
    target.value = current.value;
  }
}
