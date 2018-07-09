import { Utils } from 'app/core/utils';
import { Injectable } from '@angular/core';
import {
  IdentificationType,
  Gender,
  MaritalStatus,
  ContactType,
  AccessList,
  AcademicProgramStatus,
  CycleType,
  PaymentType,
  InstanceStatus,
} from 'app/models';
import { MenuItem } from '../../../core/models/menu-item';
import * as _ from 'underscore';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class CommonService {
  constructor(private http: HttpClient) {}

  getIdentificationTypes(): Promise<string[]> {
    return Promise.resolve(Utils.getEnumStrings(IdentificationType));
  }

  getGenders(): Promise<string[]> {
    return Promise.resolve(Utils.getEnumStrings(Gender));
  }

  getMaritalStatusList(): Promise<string[]> {
    return Promise.resolve(Utils.getEnumStrings(MaritalStatus));
  }

  getContactTypes(): Promise<string[]> {
    return Promise.resolve(Utils.getEnumStrings(ContactType));
  }

  getAccessList(): Promise<string[]> {
    return Promise.resolve(Utils.getEnumStrings(AccessList));
  }

  getAcademicProgramStatus(): Promise<string[]> {
    return Promise.resolve(Utils.getEnumStrings(AcademicProgramStatus));
  }

  getCycleTypes(): Promise<string[]> {
    return Promise.resolve(Utils.getEnumStrings(CycleType));
  }

  getPeymentTypes(): Promise<string[]> {
    return Promise.resolve(Utils.getEnumStrings(PaymentType));
  }

  getProgramInstanceStatus(): Promise<string[]> {
    return Promise.resolve(Utils.getEnumStrings(InstanceStatus));
  }

  getMenu(): Promise<MenuItem[]> {
    return this.http
      .get('/assets/config/menu-config.json')
      .toPromise()
      .then(menus => {
        return menus as MenuItem[];
      });
  }
}
