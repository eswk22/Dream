

import { Injectable } from '@angular/core';
import { Http, Response, URLSearchParams } from '@angular/http';
import { Observable }     from 'rxjs/Observable';

import { ActionTaskModel } from './actionTaskModel';

@Injectable()
export class ActionTaskService {
	private _defaultUrl = 'http://localhost:26402/api/actiontask/';

	constructor(private http: Http) { }


	save(actionTaskModel: ActionTaskModel): Observable<ActionTaskModel> {
		let url: string = this._defaultUrl + 'save';
		return this.http.post(url, actionTaskModel)
			.map(this.extractData)
			.catch(this.handleError);
	}

	executeCode(actionTaskModel: ActionTaskModel): Observable<boolean> {
		let url: string = this._defaultUrl + 'executecode';
		return this.http.post(url, actionTaskModel)
			.map(this.extractData)
			.catch(this.handleError);
	}

	getById(actionId: string): Observable<ActionTaskModel> {
		let params: URLSearchParams = new URLSearchParams();
		params.set('ActionId', actionId);
		let url: string = this._defaultUrl + 'getbyid';
		return this.http.get(url, {
			search: params
		}).map(this.extractData)
			.catch(this.handleError);
	}



	private extractData(res: Response) {
		let body = res.json();
		return body || {};
	}

	private handleError(error: any) {
		// In a real world app, we might use a remote logging infrastructure
		// We'd also dig deeper into the error to get a better message
		let errMsg = (error.message) ? error.message :
			error.status ? `${error.status} - ${error.statusText}` : 'Server error';
		console.error(errMsg); // log to console instead
		return Observable.throw(errMsg);
	}
}