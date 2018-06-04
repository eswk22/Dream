import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { UserModel, RoleModel } from './model/usermodel';
import { AppState } from '../app.service';
import { AuthHttp } from '../common/customHttp';
import { NotificationsService, NotificationComponent } from 'angular2-notifications';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

@Injectable()
export class AccountService {

    public value = 'Angular 2';
    private ServiceUrl = this.appState.ServiceUrl;
    constructor(
        public appState: AppState,
        public http: Http,
        public authHttp: AuthHttp,
        public _notification: NotificationsService
    ) {
    }

    public login(username, password): Observable<UserModel> {
       // let bodyString = JSON.stringify({ username: username, password : password, grant_type: 'password' }); // Stringify payload
        let bodyString = "username=" + username + "&password=" + password + "&grant_type=password";
        let headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' }); // ... Set content type to JSON
        let options = new RequestOptions({ headers: headers }); // Create a request option

        return this.http.post(this.ServiceUrl + '/token', bodyString, options) // ...using post request
            .map((res: Response) => {
                let token = res.json() && res.json().access_token;
                if (token) {
                    localStorage.setItem('currentUser', JSON.stringify({ res }));
                    sessionStorage.setItem('access_token', token);
                    this.appState.set('loggedInUserId', res.json().UserName);
                    return res.json();
                }
                else {
                    return null;
                }
            })
            // ...and calling .json() on the response to return data
            .catch(e => this.handleError(e, this._notification)) //...errors if any
    }


    public logout(): void {
        this.authHttp.post(this.ServiceUrl + 'api/Account/Logout', null) 
        localStorage.removeItem('currentUser');
        sessionStorage.removeItem('access_token');
        this.appState.set('loggedInUserId', undefined);
     }


    public RegisterUser(registerData): Observable<Response> {
        let bodyString = JSON.stringify({ registerData });
        return this.http.post(this.ServiceUrl + 'api/Account/Register', registerData)
            .map((res: Response) => res.json())
            .catch(e => this.handleError(e, this._notification)) //...errors if any
    }

    //    localStorage.setItem('id_token', response.json().id_token);


   

    public getAllRoles(): Observable<Array<RoleModel>> {
        return this.http.post(this.ServiceUrl + 'api/Account/getUserRoles', null)
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
