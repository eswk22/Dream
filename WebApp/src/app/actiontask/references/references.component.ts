import { Component, ViewEncapsulation, OnInit, ViewChild, ElementRef, AfterViewInit, Input, Output, forwardRef, EventEmitter } from '@angular/core';
import { CompilerArgs, CompilationResult, CompilationResultDiagnostic } from './../model/compilerargs';
import { ActionTaskService } from './../actiontask.service';

import { GridOptions } from "ag-grid/main";


declare const require: any;

@Component({
	selector: 'actiontask-references',
	templateUrl: './references.component.html',
	encapsulation: ViewEncapsulation.None,
	styles: [require('./references.component.css')],
	providers: [
		ActionTaskService
	],
})

export class ATReferenceComponent implements OnInit, AfterViewInit {
	private gridOptions: GridOptions;
	public rowData: any[];
	private columnDefs: any[];

	constructor() {
		this.gridOptions = <GridOptions>{
			onGridReady: () => {
				this.gridOptions.api.sizeColumnsToFit();
			}
		};
		this.columnDefs = [
			{ headerName: "Code", field: "Id" },
			{ headerName: "Description", field: "Message" },
			{ headerName: "Severity", field: "Severity" },
			{ headerName: "Line", field: "Start.Line" },
			{ headerName: "Column", field: "Start.Column" }
		];
	}

	ngOnInit() {
	}
	ngAfterViewInit() {
	}

	public edit() {
	}

	public copy() {
	}
	public new() {
	}
}