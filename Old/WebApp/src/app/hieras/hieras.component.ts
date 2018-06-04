import { Component, ViewEncapsulation } from '@angular/core';
import { SimpleNotificationsModule } from 'angular2-notifications';
@Component({
    selector: 'hiera',
    encapsulation: ViewEncapsulation.None,
    styles: [],
    template: `
    <ba-sidebar></ba-sidebar>
    <ba-page-top></ba-page-top>
    <div class="al-main">
      <div class="al-content">
        <ba-content-top></ba-content-top>
        <router-outlet></router-outlet>
      </div>
    </div>
    <simple-notifications [options]="options"></simple-notifications>
    <footer class="al-footer clearfix">
      <div class="al-footer-right">Created with <i class="ion-heart"></i></div>
      <div class="al-footer-main clearfix">
        <div class="al-copy">&copy; <a href="http://internal.ericsson.com">Ericsson</a> 2017</div>
      </div>
    </footer>
    <ba-back-top position="200"></ba-back-top>
    `
})
export class HieraComponent {

    constructor() {
        console.log("Hiera component");
    }

    ngOnInit() {
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

    public startSearch() {
        console.log("Search");
    }
}
