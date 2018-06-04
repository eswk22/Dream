import { Component, ViewEncapsulation } from '@angular/core';
import { SimpleNotificationsModule, NotificationComponent, NotificationsService } from 'angular2-notifications';
@Component({
    selector: 'account',
    encapsulation: ViewEncapsulation.None,
    styles: [],
    template: `
    <router-outlet></router-outlet>
    <simple-notifications [options]="options"></simple-notifications>
    `
})
export class AccountComponent {

    constructor() {
        console.log("Account component");
        
    }
    public options = {
        timeOut: 5000,
        lastOnBottom: true,
        clickToClose: true,
        maxLength: 0,
        maxStack: 7,
        showProgressBar: true,
        pauseOnHover: true,
        preventDuplicates: false,
        preventLastDuplicates: 'visible',
        rtl: false,
        animate: 'scale',
        position: ['right', 'bottom']
    };
    ngOnInit() {
    }
}
