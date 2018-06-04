import { Routes } from '@angular/router';
import { HomeComponent } from './home';
import { AccountComponent } from './account';
import { EditorComponent } from './editor';
import { AboutComponent } from './about';
import { NoContentComponent } from './no-content';

import { DataResolver } from './app.resolver';
import { AuthGuard } from './common/auth.guard.service';

export const ROUTES: Routes = [
	{ path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
	{
		path: 'account',
		redirectTo: 'account'
	},
	{
		path: 'editor',
		redirectTo: 'editor'
	},
	{ path: '', component: HomeComponent },
	{ path: 'home', component: HomeComponent },
	{ path: 'about', component: AboutComponent },
	{ path: 'detail', loadChildren: './+detail#DetailModule' },
	{ path: 'barrel', loadChildren: './+barrel#BarrelModule' },
	{ path: '**', component: NoContentComponent },
];
