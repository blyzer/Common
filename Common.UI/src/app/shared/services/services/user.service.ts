import { User } from 'app/models';
import { Injectable } from '@angular/core';
import { ModelService, BaseHttpService } from 'app/core/http';

@Injectable()
export class UserService extends ModelService {
  static baseUri = 'api/users';
  constructor(http: BaseHttpService) {
    super(http, UserService.baseUri);
  }

  public getAll(count?: number, pageNumber?: number): Promise<User[]> {
    console.log(this.baseUri);
    return this.http.get(`${this.baseUri}?count=${count}&pageNumber=${pageNumber}`);
  }

  public get(userName: string): Promise<User> {
    return this.http.get(`${this.baseUri}/${userName}`);
  }

  public add(user: User): Promise<void> {
    return this.http.post(this.baseUri, user);
  }

  public edit(userName: string, user: User): Promise<void> {
    return this.http.put(`${this.baseUri}/${userName}`, user);
  }
}
