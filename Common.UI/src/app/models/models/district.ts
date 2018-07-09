import {Municipality} from '../index';
export class District {
  public districtId: number;
  public municipalityId: number;
  public name: string;

  public municipality: Municipality;
}
