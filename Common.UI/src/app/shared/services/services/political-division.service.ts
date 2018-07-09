import { Injectable } from '@angular/core';
import { BaseHttpService, ModelService } from 'app/core/http';
import { Province, Municipality, District } from 'app/models';

@Injectable()
export class PoliticalDivisionService extends ModelService {

  static baseUri = 'api/political-divisions';
  constructor(http: BaseHttpService) {
    super(http, PoliticalDivisionService.baseUri);
  }

  getProvinces(): Promise<Province[]> {
    return this.http.get(`${this.baseUri}/provinces`)
      .then(resp => resp)
      .catch(this.handleErrorAsPromise);
  }

  getMunicipalities(provinceName: string): Promise<Municipality[]> {
    return this.http.get(`${this.baseUri}/provinces/${provinceName}/municipalities`)
      .then(resp => resp)
      .catch(this.handleErrorAsPromise);
  }

  getDistricts(provinceName: string, MunicipalityName: string): Promise<District[]> {
    return this.http.get(`${this.baseUri}/provinces/${provinceName}/municipalities/${MunicipalityName}/districts`)
      .then(resp => resp)
      .catch(this.handleErrorAsPromise);
  }
}
