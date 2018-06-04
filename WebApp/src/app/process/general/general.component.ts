import { Component, ViewEncapsulation, OnInit, Inject, ViewChild, ElementRef, AfterViewInit, Input, Output, forwardRef, EventEmitter } from '@angular/core';
import { CompilerArgs, CompilationResult, CompilationResultDiagnostic } from './../model/compilerargs';
import { ProcessService } from './../process.service';
import { FormGroup, AbstractControl, FormBuilder, Validators, FormArray } from '@angular/forms';
import { GridOptions } from "ag-grid/main";
import { AppState } from '../../app.service';
import { SnotifyService } from 'ng-snotify';

declare const require: any;
declare var mxClient: any;
declare var mxObjectCodec: any;
declare var mxPanningManager: any; 
declare var mxEvent: any;
declare var mxEditor: any;
declare var mxLog: any;
declare var mxUtils : any;

@Component({
	selector: 'process-general',
	templateUrl: './general.component.html',
	encapsulation: ViewEncapsulation.None,
	styles: [require('./general.component.css')],
	providers: [
		ProcessService
	],
})

export class GeneralComponent implements OnInit, AfterViewInit {
	public form: FormGroup;
	private gridOptions: GridOptions;
	public rowData: any[];
	private columnDefs: any[];
	private isLatestVersion: boolean = true;

	constructor( @Inject(FormBuilder) fb: FormBuilder, public appState: AppState,
		public _processManager: ProcessService, private _notification: SnotifyService
	) {
		this.form = fb.group({
			name: ['', Validators.compose([Validators.required, Validators.minLength(4)])],
			namespace: [''],
			menu: [''],
			isactive: [true],
			summary : [ ''],
			description : ['']
		});
	}

	ngOnInit() {
	}
	ngAfterViewInit() {
	}



}