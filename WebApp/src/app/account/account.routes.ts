import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { SignupComponent } from './signup/signup.component';
import { ForgotPasswordComponent } from './forgotpassword/forgotpassword.component';
import { ResetPasswordComponent } from './resetpassword/resetpassword.component';

import { AccountComponent } from './account.component';

// noinspection TypeScriptValidateTypes
const routes: Routes = [
    {
        path: 'account',
        component: AccountComponent,
        children: [
            { path: 'login', component: LoginComponent },
            { path: 'register', component: SignupComponent },
            { path: 'forgotpassowrd', component: ForgotPasswordComponent },
            { path: 'changepassword', component: ResetPasswordComponent }
        ]
    }
];

export const routing = RouterModule.forChild(routes);
