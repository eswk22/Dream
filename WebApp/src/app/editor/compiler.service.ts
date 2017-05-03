

import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable }     from 'rxjs/Observable';

import { CompilationArguments } from './process';
import { CompilerResultModel } from '../compilerResult';

@Injectable()
export class CompilerService {
	private _compilerUrl = 'http://localhost:26402/api/compilation';

	constructor(private http: Http) { }


	compile(compileargs: CompilationArguments): Observable<CompilerResultModel> {
		return this.http.post(this._compilerUrl, compileargs)
			.map(this.extractData)
			.catch(this.handleError);
	}

	compile1(): Observable<string[]> {
		return this.http.get(this._compilerUrl)
			.map(this.extractData)
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