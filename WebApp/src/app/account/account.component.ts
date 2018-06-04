import { Component, ViewEncapsulation } from '@angular/core';
@Component({
    selector: 'account',
    encapsulation: ViewEncapsulation.None,
    styles: [],
    template: `
    <router-outlet></router-outlet>
    <ng-snotify></ng-snotify>
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
