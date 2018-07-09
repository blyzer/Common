import {District, Province} from '../index';
export class Municipality {
  public municipalityId: number;
  public provinceId: number;
  public name: string;

  public province: Province;
  public district: District[];
}
