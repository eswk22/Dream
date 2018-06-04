
export class UserModel {
    constructor(
     ) { }

    public Email: string;
    public UserName: string;
    public FirstName: string;
    public LastName: string;
    public access_token: string;
    public Roles: Array<string>;
}

export class RoleModel {
    constructor(
       
    ) { }
    public Id: string;
    public Name: string;
    public Rank: string;
}


export class RegisterBindingModel {
    constructor(
    ) { }

    public Email: string;
    public UserName: string;
    public FirstName: string;
    public LastName: string;
    public Phone: string;
    public Password: string;
    public ConfirmPassword: string;
    public selectedRoles: Array<string>;
}