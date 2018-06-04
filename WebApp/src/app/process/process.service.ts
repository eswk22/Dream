import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs';
import { AppState } from '../app.service';
import { AuthHttp } from '../common/customHttp';
import { GridSearchModel } from './../common/model/grid-searchmodel';
import { SnotifyService } from 'ng-snotify';
import { ProcessGridModel, ProcesslistModel } from './model/processgridmodel';
import { map,catchError } from 'rxjs/operators';

@Injectable()
export class ProcessService {

    public value = 'Angular 2';
    private ServiceUrl = this.appState.ServiceUrl;
    constructor(
        public appState: AppState,
        public http: Http,
        public authHttp: AuthHttp,
        public _notification: SnotifyService
    ) {
    }

	public GetProcesses(args: GridSearchModel): Observable<ProcessGridModel> {
        return this.http.post(this.ServiceUrl + 'api/automation/search', args)
			.pipe(map((res: Response) => res.json())
			,catchError(e => this.handleError(e, this._notification))) //...errors if any

	}

    public handleError(error: Response | any, notification: SnotifyService) {
       
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
