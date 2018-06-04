/**
 * Angular 2 decorators and services
 */
import {
	Component,
	OnInit,
	ViewEncapsulation
} from '@angular/core';
import { AppState } from './app.service';

/**
 * App Component
 * Top Level Component
 */
@Component({
	selector: 'app',
	encapsulation: ViewEncapsulation.None,
	styleUrls: [
		'./app.component.scss'
	],
	template: `
<mat-toolbar class="mat-elevation-z6">

    <button mat-button routerLink="/">
    <mat-icon>home</mat-icon> 
        {{title}}</button>

    <!-- This fills the remaining space of the current row -->
    <span class="fill-remaining-space"></span>
    <div fxLayout="row" fxShow="false" fxShow.gt-sm>
    <button mat-menu-item routerLink="/process/general">Process</button>
    <button mat-menu-item routerLink="/process/list">Process-list</button>
   <button mat-menu-item routerLink="/general">Action</button>
    <button mat-menu-item routerLink="/list">Action-list</button>
    </div>
    <button mat-button [mat-menu-trigger-for]="menu" fxHide="false" fxHide.gt-sm>
     <mat-icon>menu</mat-icon>
    </button>

</mat-toolbar>
<mat-menu x-position="before" #menu="matMenu">
    <button mat-menu-item routerLink="/process/general">Process</button>
    <button mat-menu-item routerLink="/process/list">Process-list</button>
   <button mat-menu-item routerLink="/general">Action</button>
    <button mat-menu-item routerLink="/list">Action-list</button>
</mat-menu>       <router-outlet></router-outlet>	
	  <span defaultOverlayTarget></span>
   
<ng-snotify></ng-snotify>
  <footer>
   <pre class="app-state">Application name @ 2017</pre>
    </footer>
  `
})
export class AppComponent implements OnInit {
	public angularclassLogo = 'assets/img/angularclass-avatar.png';
	public name = 'Angular 2 Webpack Starter';
	public url = 'https://twitter.com/AngularClass';

	constructor(
		public appState: AppState
	) { }

	public ngOnInit() {
		console.log('Initial App State', this.appState.state);
	}

}

/**
 * Please review the https://github.com/AngularClass/angular2-examples/ repo for
 * more angular app examples that you may copy/paste
 * (The examples may not be updated as quickly. Please open an issue on github for us to update it)
 * For help or questions please contact us at @AngularClass on twitter
 * or our chat on Slack at https://AngularClass.com/slack-join
 */
