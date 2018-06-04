import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { AppState } from '../app.service';
import { AuthHttp } from '../common/customHttp';
import { CompilerArgs, CompilationResult } from './model/compilerargs';
import { NotificationsService, NotificationComponent } from 'angular2-notifications';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

@Injectable()
export class CompilerService {

    public value = 'Angular 2';
    private ServiceUrl = this.appState.ServiceUrl;
    constructor(
        public appState: AppState,
        public http: Http,
        public authHttp: AuthHttp,
        public _notification: NotificationsService
    ) {
    }

	public CompileCode(args: CompilerArgs): Observable<CompilationResult> {
		let bodyString = JSON.stringify({ args });
		return this.http.post(this.ServiceUrl + 'api/compilation', args)
			.map((res: Response) => res.json())
			.catch(e => this.handleError(e, this._notification)) //...errors if any
	}

    public handleError(error: Response | any, notification: NotificationsService) {
       
        // In a real world app, we might use a remote logging infrastructure
        let title: string;
        let errMsg: string;
        if (error instanceof Response) {
            const body = error.json() || '';
            title = body.error || 'Error';
            errMsg = body.error_description;  // `${error.status} - ${error.statusText || ''} ${err}`;
        } else {
            title = "Error";
            errMsg = error.message ? error.message : error.toString();
        }
        notification.error(title, errMsg);
        return Observable.throw(error);
    }
}
