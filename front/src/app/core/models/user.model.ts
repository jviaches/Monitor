export interface UserModel {
    id: string;
    email: string;
    registrationDate: Date;
    lastLoginDate: Date;
    token?: string;

}
