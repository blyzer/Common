import { UserType } from './user-type';

export class User {
    public userName: string;
    public password: string;
    public email: string;
    public firstName: string;
    public lastName: string;
    public active: boolean;
    public roleName: string;
    public userType: UserType;
}
