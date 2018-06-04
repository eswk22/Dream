import { HttpModule, Http, RequestOptions } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { SnotifyModule, SnotifyService, ToastDefaults } from 'ng-snotify';

//import { MatDialogModule  } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FlexLayoutModule } from "@angular/flex-layout";
import { MdModule } from './md.module';
import { ProcessModule } from './process';

import {
	NgModule,
	ApplicationRef
} from '@angular/core';
import {
	removeNgStyles,
	createNewHosts,
	createInputTransfer
} from '@angularclass/hmr';
import {
	RouterModule,
	PreloadAllModules
} from '@angular/router';


import { AUTH_PROVIDERS, AuthHttp, AuthConfig } from './common/customHttp';
import { AuthGuard } from './common/auth.guard.service';
/*
 * Platform and Environment providers/directives/pipes
 */
import { ENV_PROVIDERS } from './environment';
import { ROUTES } from './app.routes';
// App is our top level component
import { AppComponent } from './app.component';
import { APP_RESOLVER_PROVIDERS } from './app.resolver';
import { AppState, InternalStateType } from './app.service';
import { HomeComponent } from './home';
import { AccountComponent, AccountModule } from './account';
import { EditorComponent, EditorModule } from './editor';
import { ActionTaskModule } from './actiontask';
import { AboutComponent } from './about';
import { NoContentComponent } from './no-content';
import { XLargeDirective } from './home/x-large';
import { AgGridModule } from "ag-grid-angular/main";

import { TabsModule } from "ng2-tabs";

import '../styles/styles.scss';
import '../styles/headings.css';

import 'ag-grid-angular/main';
import 'hammerjs';

// Application wide providers
const APP_PROVIDERS = [
	...APP_RESOLVER_PROVIDERS,
	AppState
];

type StoreType = {
	state: InternalStateType,
	restoreInputValues: () => void,
	disposeOldHosts: () => void
};

/**
 * `AppModule` is the main entry point into Angular2's bootstraping process
 */
@NgModule({
	bootstrap: [AppComponent],
	declarations: [
		AppComponent,
		AboutComponent,
		HomeComponent,
		AccountComponent,
	//	EditorComponent,
		NoContentComponent,
		XLargeDirective
	],
	/**
	 * Import Angular's modules.
	 */
	imports: [
		AgGridModule.withComponents([]),
		BrowserAnimationsModule,
		BrowserModule,
		FlexLayoutModule,
        BrowserAnimationsModule,
        MdModule.forRoot(),
    //    MatDialogModule,
		FormsModule,
		HttpModule,
		AccountModule,
		EditorModule,
		ProcessModule,
		ActionTaskModule,
		TabsModule,
        SnotifyModule,
		RouterModule.forRoot(ROUTES, { useHash: true, preloadingStrategy: PreloadAllModules })
	],
	/**
	 * Expose our Services and Providers into Angular's dependency injection.
	 */
	providers: [
		AuthGuard,
		ENV_PROVIDERS,
		APP_PROVIDERS,
	    {
			provide: AuthHttp,
			useFactory: (http, options) => {
				return new AuthHttp(
					new AuthConfig({
						headerName: 'Authorization',
						headerPrefix: 'Bearer',
						tokenName: 'id_token',
						tokenGetter: (() => sessionStorage.getItem('access_token')),
						globalHeaders: [{ 'Content-Type': 'application/json' }],
					}), http, options)
			},
			deps: [Http, RequestOptions]
        },
        { provide: 'SnotifyToastConfig', useValue: ToastDefaults },
        SnotifyService
	]
})
export class AppModule {

	constructor(
		public appRef: ApplicationRef,
		public appState: AppState
	) { }

	public hmrOnInit(store: StoreType) {
		if (!store || !store.state) {
			return;
		}
		console.log('HMR store', JSON.stringify(store, null, 2));
		/**
		 * Set state
		 */
		this.appState._state = store.state;
		/**
		 * Set input values
		 */
		if ('restoreInputValues' in store) {
			let restoreInputValues = store.restoreInputValues;
			setTimeout(restoreInputValues);
		}

		this.appRef.tick();
		delete store.state;
		delete store.restoreInputValues;
	}

	public hmrOnDestroy(store: StoreType) {
		const cmpLocation = this.appRef.components.map((cmp) => cmp.location.nativeElement);
		/**
		 * Save state
		 */
		const state = this.appState._state;
		store.state = state;
		/**
		 * Recreate root elements
		 */
		store.disposeOldHosts = createNewHosts(cmpLocation);
		/**
		 * Save input values
		 */
		store.restoreInputValues = createInputTransfer();
		/**
		 * Remove styles
		 */
		removeNgStyles();
	}

	public hmrAfterDestroy(store: StoreType) {
		/**
		 * Display new elements
		 */
		store.disposeOldHosts();
		delete store.disposeOldHosts;
	}

}
