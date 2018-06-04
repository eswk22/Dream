import {
    Component, ViewEncapsulation,
    OnInit
} from '@angular/core';

import { AppState } from '../../app.service';
import { AccountService } from '../account.service';
import { FormGroup, AbstractControl, FormBuilder, Validators } from '@angular/forms';
import { EmailValidator, EqualPasswordsValidator } from '../../theme/validators';
import { RoleModel, RegisterBindingModel } from '../model/usermodel';

@Component({
    // The selector is what angular internally uses
    // for `document.querySelectorAll(selector)` in our index.html
    // where, in this case, selector is the string 'home'
    selector: 'signup',  // <home></home>
    // We need to tell Angular's Dependency Injection which providers are in our app.
    providers: [
        AccountService
    ],
    // Our list of styles in our component. We may add more to compose many styles together
    styles: [require('./signup.scss')],
    // Every Angular template is first compiled by the browser before Angular runs it's compiler
    templateUrl: './signup.html'
})
export class SignupComponent implements OnInit {
    // Set our default values
    public localState = { value: '' };
    public form: FormGroup;
    public firstname: AbstractControl;
    public lastname: AbstractControl;
    public username: AbstractControl;
    public role: AbstractControl;
    public email: AbstractControl;
    public password: AbstractControl;
    public repeatPassword: AbstractControl;
    public passwords: FormGroup;
    public allRoles: Array<RoleModel>;

    public submitted: boolean = false;

    // TypeScript public modifiers
    constructor(
        public appState: AppState,
        public accountManager: AccountService,
        fb: FormBuilder
    ) { 
        this.form = fb.group({
            'firstname': ['', Validators.compose([Validators.required, Validators.minLength(4)])],
            'lastname': ['', Validators.compose([Validators.required, Validators.minLength(4)])],
            'username': ['', Validators.compose([Validators.required, Validators.minLength(4)])],
            'email': ['', Validators.compose([Validators.required, EmailValidator.validate])],
            'passwords': fb.group({
                'password': ['', Validators.compose([Validators.required, Validators.minLength(4)])],
                'repeatPassword': ['', Validators.compose([Validators.required, Validators.minLength(4)])]
            }, { validator: EqualPasswordsValidator.validate('password', 'repeatPassword') }),
            'role' : ['',Validators.required]

        });

        this.firstname = this.form.controls['firstname'];
        this.lastname = this.form.controls['lastname'];
        this.username = this.form.controls['username'];
        this.role = this.form.controls['role'];
        this.email = this.form.controls['email'];
        this.passwords = <FormGroup>this.form.controls['passwords'];
        this.password = this.passwords.controls['password'];
        this.repeatPassword = this.passwords.controls['repeatPassword'];

        accountManager.getAllRoles()
            .subscribe(result => {
                this.allRoles = result;
            });

    }

    public onSubmit(values: Object): void {
        this.submitted = true;
        if (this.form.valid) {
            var registerData = new RegisterBindingModel();
            registerData.ConfirmPassword = values['passwords']['repeatPassword'];
            registerData.Email = values['email'];
            registerData.FirstName = values['firstname'];
            registerData.LastName = values['lastname'];
            registerData.Password = values['passwords']['password'];
            registerData.Phone = "000000";
            registerData.selectedRoles = [];
            registerData.selectedRoles.push(values['role']);
            registerData.UserName = values['username'];
            
            this.accountManager.RegisterUser(registerData)
                .subscribe(result => {
                    console.log(result);
                });

        }
    }

    private onRoleChange(newValue) {
        console.log(newValue);
       
    }

    public ngOnInit() {
        console.log('hello `Home` component');
        // this.title.getData().subscribe(data => this.data = data);
    }

    
}
