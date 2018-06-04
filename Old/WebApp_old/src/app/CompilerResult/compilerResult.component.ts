import { Component, ViewChild,Input } from '@angular/core';


import { AppState } from '../app.service';

import {CompilerResultModel} from './CompilerResultModel';

import {AgGridNg2} from 'ag-grid-ng2/main';




@Component({
	// The selector is what angular internally uses
	// for `document.querySelectorAll(selector)` in our index.html
	// where, in this case, selector is the string 'home'
	selector: 'compilerresult',  // <home></home>
	// We need to tell Angular's Dependency Injection which providers are in our app.
	providers: [
	],
	// We need to tell Angular's compiler which directives are in our template.
	// Doing so will allow Angular to attach our behavior to an element
	directives: [
        CompilerResult, AgGridNg2
	],
	// We need to tell Angular's compiler which custom pipes are in our template.
	pipes: [],
	// Our list of styles in our component. We may add more to compose many styles together
	styleUrls: ['./CompilerResult.style.css'],
	// Every Angular template is first compiled by the browser before Angular runs it's compiler
	templateUrl: './CompilerResult.template.html'
})
export class CompilerResult {
	// Set our default values
    private gridOptions: GridOptions;
    private showGrid: boolean;
    private rowCount: string;
    private columnDefs: any[];
	value: string;
	@Input('Result') Result: CompilerResultModel;
	

	// TypeScript public modifiers
	constructor(public appState: AppState) {
		//	  this.localState.value = 'public void';
		this.Result = {
			IsSuccess: true,
			Decompiled: '',
			Errors: [],
			Warnings: [],
			Infos: []
        };

        this.gridOptions = <GridOptions>{};
        this.createColumnDefs();
        this.showGrid = true;

	}

    createColumnDefs() {
        this.columnDefs = [{ headerName: "Error", width: 80 },
            { headerName: "Code", field: "Id", width: 120 },
            { headerName: "Description", field: "Message", width: 450 },
            { headerName: "Line", field: "Start.Line", width: 100 }
        ]
    }

	
	onvaluechange(event) {
		console.log(event);
	}

	ngOnInit() {
		console.log('hello `Home` component');
		// this.title.getData().subscribe(data => this.data = data);
	}





}



