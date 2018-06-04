import { Component, OnInit, ViewEncapsulation, Input, Output, ElementRef, EventEmitter } from '@angular/core';

import { GridOptions } from "ag-grid/main";
import { CompilationResultDiagnostic } from '../model/compilerargs';
//import exportedStyles from '!!css-loader!../../../styles/theme-material.css';

/*
 * We're loading this component asynchronously
 * We are using some magic with es6-promise-loader that will wrap the module with a Promise
 * see https://github.com/gdi2290/es6-promise-loader for more info
 */

console.log('`ErrorPanel` component loaded asynchronously');

@Component({
	selector: 'error-panel',
	encapsulation: ViewEncapsulation.None,
	styles: [require('./error-panel.component.scss')],
	templateUrl: './error-panel.component.html'
})
export class ErrorPanelComponent implements OnInit {

	@Output() select = new EventEmitter();
	public ngOnInit() {
		console.log('hello `ErrorPanel` component');
	}
	@Input() set errors(v: any) {
		console.log(v);
		if (v !== this._errors) {
			this._errors = v;
			console.log(v);
		}
	}
	@Input() set warnings(v: Array<CompilationResultDiagnostic>) {
		if (v !== this._warnings) {
			this._warnings = v;
			console.log(v);
		}
	}
	private _errors: Array<CompilationResultDiagnostic>;
	private _warnings: Array<CompilationResultDiagnostic>;
	private gridOptions: GridOptions;
	public rowData: any[];
	private columnDefs: any[];

	constructor() {
		// we pass an empty gridOptions in, so we can grab the api out
		this.gridOptions = <GridOptions>{
			onGridReady: () => {
				this.gridOptions.api.sizeColumnsToFit();
			}
		};
		this.columnDefs = [
			{ headerName: "Make", field: "make" },
			{ headerName: "Model", field: "model" },
			{ headerName: "Price", field: "price" }
		];
		this.rowData = [
			{ make: "Toyota", model: "Celica", price: 35000 },
			{ make: "Ford", model: "Mondeo", price: 32000 },
			{ make: "Porsche", model: "Boxter", price: 72000 }
		];
	}

}
