import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home';
//import { LoginComponent, SignupComponent, ForgotPasswordComponent, ResetPasswordComponent } from './account';
import { AccountComponent } from './account';
import { EditorComponent } from './editor';
import { HieraComponent } from './hieras';
import { FlowChart } from './flowchart';
import { AboutComponent } from './about';
import { NoContentComponent } from './no-content';

import { DataResolver } from './app.resolver';

import { AuthGuard } from './common/auth.guard.service';

export const ROUTES: Routes = [
    // { path: '', component: LoginComponent },
    { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
    {
        path: 'account',
        redirectTo: 'account'
    },
    {
        path: 'editor',
        redirectTo: 'editor'
    },
    {
        path: 'flowchart',
        redirectTo: 'flowchart'
    },
    {
        path: 'hiera',
        redirectTo: 'hiera',
        canActivate: [AuthGuard]
    },
   { path: 'about', component: AboutComponent, canActivate: [AuthGuard] },
    { path: 'detail', loadChildren: './+detail#DetailModule', canActivate: [AuthGuard] },
    { path: 'barrel', loadChildren: './+barrel#BarrelModule', canActivate: [AuthGuard] },
    { path: 'nocontent', component: NoContentComponent },
    { path: '**', redirectTo: 'account/login' },
];


