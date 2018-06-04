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
    selector: 'forgotpassword',  // <home></home>
    // We need to tell Angular's Dependency Injection which providers are in our app.
	providers: [
        AccountService
    ],
    // Our list of styles in our component. We may add more to compose many styles together
	styles: [require('./forgotpassword.css')],
    // Every Angular template is first compiled by the browser before Angular runs it's compiler
    templateUrl: './forgotpassword.component.html'
})
export class ForgotPasswordComponent implements OnInit {
    // Set our default values
    public localState = { value: '' };
    forgotForm: FormGroup;
    // TypeScript public modifiers
    constructor(
        public fb: FormBuilder,
        public router: Router,
        public appState: AppState,
        public accountManager: AccountService
    ) {
        this.forgotForm = this.fb.group({
            username: ["", Validators.required]
        });
    }




    public ngOnInit() {
        console.log('hello `forgot` component');
        // this.title.getData().subscribe(data => this.data = data);
    }

    login(event) {
        var username = this.forgotForm.controls["username"].value;
        console.log("test");
        event.preventDefault();
        this.accountManager.login(username,'')
            .subscribe(result => {
                if (result.access_token != null) {
                    this.router.navigate(['home']);
                }
            });

    }

}
