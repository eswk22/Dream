import { NgModule, ApplicationRef } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { removeNgStyles, createNewHosts } from '@angularclass/hmr';

import { MdModule } from './md.module';

/*
 * Platform and Environment providers/directives/pipes
 */
import { ENV_PROVIDERS } from './environment';
import { approutes } from './app.routes';
// App is our top level component
import { App } from './app.component';
import { APP_RESOLVER_PROVIDERS } from './app.resolver';
import { AppState } from './app.service';
import { Home } from './home';
import { About } from './about';
import { editor } from './editor';
import { actionTask } from './actiontask';
import { actionTasklist } from './actiontasklist';
import { NoContent } from './no-content';

//import {Codemirror} from 'ng2-codemirror';

// Application wide providers
const APP_PROVIDERS = [
	...APP_RESOLVER_PROVIDERS,
	AppState
];

/**
 * `AppModule` is the main entry point into Angular2's bootstraping process
 */
@NgModule({
	bootstrap: [App],
	declarations: [
		App,
		About,
		Home,
		editor,
		actionTasklist,
		actionTask,
		NoContent
	],
	imports: [ // import Angular's modules
		BrowserModule,
		FormsModule,
		HttpModule,
	//	codemirror,
		ReactiveFormsModule,
		RouterModule.forRoot(approutes, { useHash: false }),
		MdModule.forRoot()
	],
	providers: [ // expose our Services and Providers into Angular's dependency injection
		ENV_PROVIDERS,
		APP_PROVIDERS
	]
})
export class AppModule {
	constructor(public appRef: ApplicationRef, public appState: AppState) { }
	hmrOnInit(store) {
		if (!store || !store.state) return;
		console.log('HMR store', store);
		this.appState._state = store.state;
		this.appRef.tick();
		delete store.state;
	}
	hmrOnDestroy(store) {
		var cmpLocation = this.appRef.components.map(cmp => cmp.location.nativeElement);
		// recreate elements
		var state = this.appState._state;
		store.state = state;
		store.disposeOldHosts = createNewHosts(cmpLocation)
		// remove styles
		removeNgStyles();
	}
	hmrAfterDestroy(store) {
		// display new elements
		store.disposeOldHosts()
		delete store.disposeOldHosts;
	}
}