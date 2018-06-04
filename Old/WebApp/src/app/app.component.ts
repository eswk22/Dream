import { Routes } from '@angular/router';
import './app.loader.ts';
import { BaImageLoaderService, BaThemePreloader, BaThemeSpinner } from './theme/services';
import { layoutPaths } from './theme/theme.constants';
import { BaThemeConfig } from './theme/theme.config';
import { BaMenuService } from './theme';
import { ComponentsHelper } from 'ng2-bootstrap';
import { MENU } from './app.menu';

/*
 * Angular 2 decorators and services
 */
import {
	Component,
	OnInit,
	ViewEncapsulation, ViewContainerRef
} from '@angular/core';
import { AppState } from './app.service';

/* <nav>
   <a [routerLink]=" ['./'] " routerLinkActive="active">
	 Index
   </a>
   <a [routerLink]=" ['./home'] " routerLinkActive="active">
	 Home
   </a>
   <a [routerLink]=" ['./detail'] " routerLinkActive="active">
	 Detail
   </a>
   <a [routerLink]=" ['./barrel'] " routerLinkActive="active">
	 Barrel
   </a>
   <a [routerLink]=" ['./about'] " routerLinkActive="active">
	 About
   </a>
 </nav> */

/*  <pre class="app-state">this.appState.state = {{ appState.state | json }}</pre>

  <footer>
	<span>WebPack Angular 2 Starter by <a [href]="url">@AngularClass</a></span>
	<div>
	  <a [href]="url">
		<img [src]="angularclassLogo" width="25%">
	  </a>
	</div>
  </footer> */
/*
 * App Component
 * Top Level Component
 */
@Component({
	selector: 'app',
	encapsulation: ViewEncapsulation.None,
    styles: [require('./app.scss')],
    providers: [
        BaImageLoaderService, BaThemePreloader, BaThemeSpinner, BaThemeConfig, BaMenuService
    ],
	template: `


  <main [ngClass]="{'menu-collapsed': isMenuCollapsed}" baThemeRun>
      <div class="additional-bg"></div>
      <router-outlet></router-outlet>
    </main>


  `
})
export class AppComponent implements OnInit {
	public angularclassLogo = 'assets/img/angularclass-avatar.png';
	public name = 'Angular 2 Webpack Starter';
	public url = 'https://twitter.com/AngularClass';


	constructor(
		public appState: AppState,
		private _imageLoader: BaImageLoaderService,
		private _spinner: BaThemeSpinner,
		private _config: BaThemeConfig,
		private _menuService: BaMenuService,
		private viewContainerRef: ViewContainerRef
	) {

		this._menuService.updateMenuByRoutes(<Routes>MENU);

		this._fixModals();

		this._loadImages();

		this.appState.subscribe('menu.isCollapsed', (isCollapsed) => {
			this.isMenuCollapsed = isCollapsed;
		});
	}

	public ngOnInit() {

		console.log('Initial App State', this.appState.state);
	}

	isMenuCollapsed: boolean = false;



	public ngAfterViewInit(): void {
		// hide spinner once all loaders are completed
		BaThemePreloader.load().then((values) => {
			this._spinner.hide();
		});
	}

    private _loadImages(): void {
        console.log(layoutPaths.images.root);
		// register some loaders
		BaThemePreloader.registerLoader(this._imageLoader.load(layoutPaths.images.root + 'sky-bg.jpg'));
	}

	private _fixModals(): void {
		ComponentsHelper.prototype.getRootViewContainerRef = function () {
			// https://github.com/angular/angular/issues/9293
			if (this.root) {
				return this.root;
			}
			var comps = this.applicationRef.components;
			if (!comps.length) {
				throw new Error("ApplicationRef instance not found");
			}
			try {
				/* one more ugly hack, read issue above for details */
				var rootComponent = this.applicationRef._rootComponents[0];
				this.root = rootComponent._component.viewContainerRef;
				return this.root;
			}
			catch (e) {
				throw new Error("ApplicationRef instance not found");
			}
		};
	}

}

/*
 * Please review the https://github.com/AngularClass/angular2-examples/ repo for
 * more angular app examples that you may copy/paste
 * (The examples may not be updated as quickly. Please open an issue on github for us to update it)
 * For help or questions please contact us at @AngularClass on twitter
 * or our chat on Slack at https://AngularClass.com/slack-join
 */
