import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { HieraModel } from './model/hieramodel';
import { ParamModel, Param2HieraLevelModel } from './model/parammodel';
import { AppState } from '../app.service';
import { AuthHttp } from '../common/customHttp';
import { NotificationsService  } from 'angular2-notifications';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

@Injectable()
export class HieraService {

    private ServiceUrl = this.appState.ServiceUrl;
    constructor(
        public appState: AppState,
        public http: Http,
        public authHttp: AuthHttp,
        private _notification: NotificationsService
    ) { }

	public getHiera(name: string, version: number): Observable<HieraModel> {
		let params: URLSearchParams = new URLSearchParams();
		params.set('name', name);
        params.set('version', version.toString());
        return this.authHttp.get(this.ServiceUrl + 'api/Hiera/GetHieraData', { search: params })
			.map(this.extractData) // ...and calling .json() on the response to return data
            .catch(e => this.handleError(e, this._notification)) //...errors if any
	}

    public getHieraVersions(name: string): Observable<string[]> {
    	let params: URLSearchParams = new URLSearchParams();
		params.set('name', name);
        return this.authHttp.get(this.ServiceUrl + 'api/Hiera/GetVersions', { search: params })
			.map(this.extractData) // ...and calling .json() on the response to return data
            .catch(e => this.handleError(e, this._notification)) //...errors if any
	}

    public saveHiera(hiera: HieraModel) :Observable<HieraModel> {
    //    let bodyString = JSON.stringify({ _hiera: hiera });
    //    console.log(bodyString)
   //     let headers = new Headers({ 'Content-Type': 'application/json; charset=utf-8' }); // ... Set content type to JSON
   //     let options = new RequestOptions({ headers: headers }); // Create a request option
        return this.authHttp.post(this.ServiceUrl + 'api/Hiera/SaveHiera', hiera)
			.map(this.extractData) // ...and calling .json() on the response to return data
            .catch(e => this.handleError(e, this._notification)) //...errors if any
	}


    private extractData(res: Response) {
        
        let body = res.json();
    	return body || {};
	}

    public handleError(error: Response | any, notification: NotificationsService) {

        // In a real world app, we might use a remote logging infrastructure
        let title: string;
        let errMsg: string;
        if (error instanceof Response) {
            const body = error.text() || '';
            title = 'Error';
            errMsg = body;  // `${error.status} - ${error.statusText || ''} ${err}`;
        } else {
            title = "Error";
            errMsg = error.message ? error.message : error.toString();
        }
        notification.error(title, errMsg);
        return Observable.throw(error);
    }


    public getParameter(ParameterId: Number): Observable<ParamModel> {
        let params: URLSearchParams = new URLSearchParams();
        params.set('ParameterId', ParameterId.toString());
        return this.authHttp.get(this.ServiceUrl + 'api/Parameter/GetParameter', { search: params })
            .map(this.extractData) 
            .catch(e => this.handleError(e, this._notification)) 
    }
    public getParameters(ParameterId: Number): Observable<Array<ParamModel>> {
        let params: URLSearchParams = new URLSearchParams();
        params.set('ParameterId', ParameterId.toString());
        return this.authHttp.get(this.ServiceUrl + 'api/Parameter/GetParameters', { search: params })
            .map(this.extractData)
            .catch(e => this.handleError(e, this._notification))
    }
    public getParam2Hiera(ParameterId: Number): Observable<Array<Param2HieraLevelModel>> {
        let params: URLSearchParams = new URLSearchParams();
        params.set('ParameterId', ParameterId.toString());
        return this.authHttp.get(this.ServiceUrl + 'api/Parameter/GetParam2Hiera', { search: params })
            .map(this.extractData) 
            .catch(e => this.handleError(e, this._notification))
    }

    public saveParameter(param: ParamModel): Observable<ParamModel> {
        return this.authHttp.post(this.ServiceUrl + 'api/Parameter/SaveParameter', param)
            .map(this.extractData) 
            .catch(e => this.handleError(e, this._notification))
    }

    public saveParameters(param: Array<ParamModel>): Observable<Array<ParamModel>> {
        return this.authHttp.post(this.ServiceUrl + 'api/Parameter/SaveParameters', param)
            .map(this.extractData)
            .catch(e => this.handleError(e, this._notification))
    }

    public saveParam2Hiera(param2Hiera: Array<Param2HieraLevelModel>): Observable<Array<Param2HieraLevelModel>> {
        return this.authHttp.post(this.ServiceUrl + 'api/Parameter/SaveParam2Hieras', param2Hiera)
            .map(this.extractData)
            .catch(e => this.handleError(e, this._notification))
    }

}
