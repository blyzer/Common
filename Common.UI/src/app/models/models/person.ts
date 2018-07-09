import { Address } from './address';
import {
  IdentificationType,
  MaritalStatus,
  ContactInformation,
  Gender
} from '../index';

export class Person {
  firstName: string;
  lastName: string;
  dateOfBirth: Date;
  gender: Gender;
  identificationType: IdentificationType;
  identificationNumber: string;
  maritalStatus: MaritalStatus;
  nationality: string;
  email: string;

  address: Address = new Address();
  contactInformations: ContactInformation[] = [];
}
