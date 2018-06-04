import {
    Component,
    OnInit
} from '@angular/core';

import { AppState } from '../../app.service';
import { AccountService } from '../account.service';
import { Router } from '@angular/router';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';


@Component({
    // The selector is what angular internally uses
    // for `document.querySelectorAll(selector)` in our index.html
    // where, in this case, selector is the string 'home'
    selector: 'login',  // <home></home>
    // We need to tell Angular's Dependency Injection which providers are in our app.
    providers: [
        AccountService
    ],
    // Our list of styles in our component. We may add more to compose many styles together
    styles: [require('./login.css')],
    // Every Angular template is first compiled by the browser before Angular runs it's compiler
    templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {
    // Set our default values
    public localState = { value: '' };
    loginForm: FormGroup;
    // TypeScript public modifiers
    constructor(
        public fb: FormBuilder,
        public router: Router,
        public appState: AppState,
        public accountManager: AccountService
    ) {
        this.loginForm = this.fb.group({
            username: ["", Validators.required],
            password: ["", Validators.required]
        });
    }




    public ngOnInit() {
        console.log('hello `login` component');
        // this.title.getData().subscribe(data => this.data = data);
    }

    login(event) {
        var username = this.loginForm.controls["username"].value;
        var password = this.loginForm.controls["password"].value;
        event.preventDefault();
        this.accountManager.login(username, password)
            .subscribe(result => {
               
                console.log(result);
                if (result.access_token != null) {
                    this.router.navigate(['hiera/edit']);
                }
            });

    }

}
