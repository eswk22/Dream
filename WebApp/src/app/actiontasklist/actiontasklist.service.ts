

import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable }     from 'rxjs/Observable';

import { ActionTasklistModel } from './actionTasklistModel';

@Injectable()
export class ActionTasklistService {
	private _defaultUrl = 'http://localhost:26402/api/actiontask/';

	constructor(private http: Http) { }



	getAll(): Observable<ActionTasklistModel[]> {
		let url: string = this._defaultUrl + 'get';
		return this.http.get(url).map(this.extractData)
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