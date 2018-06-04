import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import { Observable } from 'rxjs';
import { AppState } from '../app.service';
import { AuthHttp } from '../common/customHttp';
import { CompilerArgs, CompilationResult } from './model/compilerargs';

import { LockActionTaskModel, ActionTaskModel, CodeModel, ActionTasklistModel, ActionTaskGridModel } from './model/actiontaskmodels';
import { RolePermission } from './model/rolepermissionmodel';
import { ParameterModel } from './../common/model/parametermodel';
import { SnotifyService } from 'ng-snotify';
import { GridSearchModel } from './../common/model/grid-searchmodel';
import { map,catchError  } from 'rxjs/operators';

@Injectable()
export class ActionTaskService {

	public value = 'Angular 2';
	private ServiceUrl = this.appState.ServiceUrl;
	constructor(
		public appState: AppState,
		public http: Http,
		public authHttp: AuthHttp,
		public _notification: SnotifyService
	) {
	}

	public CompileCode(args: CompilerArgs): Observable<CompilationResult> {
		let bodyString = JSON.stringify({ args });
		return this.http.post(this.ServiceUrl + 'api/compilation', args)
			.pipe(map((res: Response) => res.json())
            ,catchError(e => this.handleError(e, this._notification))) //...errors if any
	}



	public GetActionTask(actiontaskId: string): Observable<ActionTaskModel> {
		let params: URLSearchParams = new URLSearchParams();
		params.set('Id', actiontaskId);
		return this.http.get(this.ServiceUrl + 'api/actiontask/GetbyId', { search: params })
			.pipe(map((res: Response) => res.json())
            ,catchError(e => this.handleError(e, this._notification))) //...errors if any
    }



    public DeleteActionTask(actiontaskId: string): Observable<ActionTaskModel> {
        let params: URLSearchParams = new URLSearchParams();
        params.set('Id', actiontaskId);
        return this.http.get(this.ServiceUrl + 'api/actiontask/DeletebyId', { search: params })
            .pipe(map((res: Response) => res.json())
            , catchError(e => this.handleError(e, this._notification))) //...errors if any
    }

	public SaveParameters(args: Array<ParameterModel>): Observable<boolean> {
		let bodyString = JSON.stringify({ args });
		return this.http.post(this.ServiceUrl + 'api/actiontask/saveParameters', args)
			.pipe(map((res: Response) => res.json())
            , catchError(e => this.handleError(e, this._notification))) //...errors if any
	}


	public SaveRemoteCode(args: CodeModel): Observable<boolean> {
		let bodyString = JSON.stringify({ args });
		return this.http.post(this.ServiceUrl + 'api/actiontask/saveRemoteCode', args)
			.pipe(map((res: Response) => res.json())
            , catchError(e => this.handleError(e, this._notification))) //...errors if any
	}

	public SaveLocalCode(args: CodeModel): Observable<boolean> {
		let bodyString = JSON.stringify({ args });
		return this.http.post(this.ServiceUrl + 'api/actiontask/saveLocalCode', args)
			.pipe(map((res: Response) => res.json())
            , catchError(e => this.handleError(e, this._notification))) //...errors if any
	}

	public SaveActionTask(args: ActionTaskModel): Observable<ActionTaskModel> {
		let bodyString = JSON.stringify({ args });
		return this.http.post(this.ServiceUrl + 'api/actiontask/save', args)
			.pipe(map((res: Response) => res.json())
            , catchError(e => this.handleError(e, this._notification))) //...errors if any
	}

	public PublishActionTask(args: ActionTaskModel, comment: string): Observable<ActionTaskModel> {
		let bodyString = JSON.stringify({ args });
		let params: URLSearchParams = new URLSearchParams();
		params.set('comment', comment);

		return this.http.post(this.ServiceUrl + 'api/actiontask/publish', args, { search: params })
			.pipe(map((res: Response) => res.json())
            , catchError(e => this.handleError(e, this._notification))) //...errors if any
	}


	public LockActionTask(args: LockActionTaskModel): Observable<boolean> {
		let bodyString = JSON.stringify({ args });
		return this.http.post(this.ServiceUrl + 'api/actiontask/lockActionTask', args)
			.pipe(map((res: Response) => res.json())
            , catchError(e => this.handleError(e, this._notification))) //...errors if any
	}


	public EditActionTask(actionTaskId: string,name : string, namespace : string): Observable<ActionTaskModel> {
		let params: URLSearchParams = new URLSearchParams();
		params.set('actiontaskid', actionTaskId);
		params.set('name', name);
		params.set('ATnamespace', namespace);
		return this.http.get(this.ServiceUrl + 'api/actiontask/edit', { search: params })
			.pipe(map((res: Response) => res.json())
            , catchError(e => this.handleError(e, this._notification))) //...errors if any
    }

   

	public GetActionTasks(args: GridSearchModel): Observable<ActionTaskGridModel> {
		return this.http.post(this.ServiceUrl + 'api/actiontask/search', args)
			.pipe(map((res: Response) => res.json())
            , catchError(e => this.handleError(e, this._notification))) //...errors if any

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
