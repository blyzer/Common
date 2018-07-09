import { ModelService, BaseHttpService } from 'app/core/http';
import { Injectable } from '@angular/core';
import { Role } from 'app/models';
import { FormControl } from '@angular/forms';

@Injectable()
export class RoleService extends ModelService {
  static baseUri = 'api/roles';

  constructor(http: BaseHttpService) {
    super(http, RoleService.baseUri);
  }

  getAll(): Promise<Role[]> {
    return this.http.get(this.baseUri)
      .catch(this.handleErrorAsPromise);
  }

  get(name: string): Promise<Role> {
    return this.http.get(`${this.baseUri}/${name}`)
      .catch(this.handleErrorAsPromise);
  }

  add(role: Role): Promise<void> {
    return this.http.post(`${this.baseUri}`, role)
      .catch(this.handleErrorAsPromise);
  }

  edit(role: Role): Promise<void> {
    return this.http.put(`${this.baseUri}/${role.name}`, role)
      .catch(this.handleErrorAsPromise);
  }
}
