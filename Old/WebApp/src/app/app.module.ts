import { BrowserModule } from '@angular/platform-browser';
//import { WorkerAppModule } from '@angular/platform-webworker';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule, Http, RequestOptions } from '@angular/http';
import { SimpleNotificationsModule, PushNotificationsModule } from 'angular2-notifications';

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



/*
 * Platform and Environment providers/directives/pipes
 */

import { AUTH_PROVIDERS,AuthHttp, AuthConfig } from './common/customHttp';
import { AuthGuard } from './common/auth.guard.service';

import { ENV_PROVIDERS } from './environment';
import { ROUTES } from './app.routes';
// App is our top level component
import { AppComponent } from './app.component';
import { APP_RESOLVER_PROVIDERS } from './app.resolver';
import { AppState, InternalStateType } from './app.service';
import { HomeComponent } from './home';
import { AccountComponent } from './account';
import { EditorComponent } from './editor';
//import { ErrorPanelComponent } from './Editor/+error-panel';
import { HieraComponent, HieraModule } from './hieras';
import { AboutComponent } from './about';
import { NoContentComponent } from './no-content';
import { XLargeDirective } from './home/x-large';
import { NgaModule } from './theme/nga.module';
import { AccountModule } from './account/account.module';
import { EditorModule } from './editor/editor.module';
// ag-grid
import { AgGridModule } from "ag-grid-angular/main";

import '../styles/styles.scss';
//import '../styles/headings.css';

import { MonacoEditorComponent } from 'ng2-monaco-editor';
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
        EditorComponent,
        MonacoEditorComponent,
        //     LoginComponent, SignupComponent, ForgotPasswordComponent, ResetPasswordComponent,
        HieraComponent,
		NoContentComponent,
	//	ErrorPanelComponent,
        XLargeDirective,
    ],
	imports: [ // import Angular's modules
		AgGridModule.withComponents([]),
		//WorkerAppModule,
		BrowserModule,
        FormsModule,
        NgaModule.forRoot(),
        HttpModule,
        ReactiveFormsModule,
        AccountModule,
        EditorModule,
        HieraModule,
        SimpleNotificationsModule, PushNotificationsModule,
        RouterModule.forRoot(ROUTES, { useHash: true, preloadingStrategy: PreloadAllModules })
    ],
    providers: [ // expose our Services and Providers into Angular's dependency injection
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
        }

    ]
})





export class AppModule {

    //constructor(
    //    public appRef: ApplicationRef,
    //    public appState: AppState
    //) { }

    //public hmrOnInit(store: StoreType) {
    //    if (!store || !store.state) {
    //        return;
    //    }
    //    console.log('HMR store', JSON.stringify(store, null, 2));
    //    // set state
    //    this.appState._state = store.state;
    //    // set input values
    //    if ('restoreInputValues' in store) {
    //        let restoreInputValues = store.restoreInputValues;
    //        setTimeout(restoreInputValues);
    //    }

    //    this.appRef.tick();
    //    delete store.state;
    //    delete store.restoreInputValues;
    //}

    //public hmrOnDestroy(store: StoreType) {
    //    const cmpLocation = this.appRef.components.map((cmp) => cmp.location.nativeElement);
    //    // save state
    //    const state = this.appState._state;
    //    store.state = state;
    //    // recreate root elements
    //    store.disposeOldHosts = createNewHosts(cmpLocation);
    //    // save input values
    //    store.restoreInputValues = createInputTransfer();
    //    // remove styles
    //    removeNgStyles();
    //}

    //public hmrAfterDestroy(store: StoreType) {
    //    // display new elements
    //    store.disposeOldHosts();
    //    delete store.disposeOldHosts;
    //}

}
