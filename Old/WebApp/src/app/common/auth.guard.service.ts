import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';


@Injectable()
export class AuthGuard implements CanActivate {
    constructor(private router: Router) { }

    canActivate() {
        // Check to see if a user has a valid JWT
        if (sessionStorage.getItem('access_token')) {
            // logged in so return true
            return true;
        }

        // If not, they redirect them to the login page
        this.router.navigate(['account/login']);
        return false;
    }
    
}