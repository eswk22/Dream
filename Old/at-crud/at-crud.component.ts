import { Component, ViewEncapsulation, OnInit, Inject, ViewChild, ElementRef, AfterViewInit, Input, Output, forwardRef, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { SimpleNotificationsModule, NotificationComponent, NotificationsService } from 'angular2-notifications';
import { CompilerArgs, CompilationResult, CompilationResultDiagnostic } from './model/compilerargs';
//import { CompilerService } from './editor.service';
import { FormGroup, AbstractControl, FormBuilder, Validators, FormArray } from '@angular/forms';
import { AppState } from '../../app.service';

import { GridOptions } from "ag-grid/main";



declare const require: any;

@Component({
	selector: 'actiontask-crud',
	templateUrl: './at-crud.component.html',
	encapsulation: ViewEncapsulation.None,
	styles: [require('./at-crud.component.css')],
	providers: [
		//		CompilerService
	],
})

export class ActionTaskComponent implements OnInit, AfterViewInit {
	public form: FormGroup;
	public taskId: string;
	constructor( @Inject(FormBuilder) fb: FormBuilder, public appState: AppState) {
		this.form = fb.group({
			name: ['', Validators.compose([Validators.required, Validators.minLength(4)])],
			namespace: ['', Validators.compose([Validators.required, Validators.minLength(4)])],
			menupath: ['', Validators.compose([Validators.required, Validators.minLength(4)])],
			timeout :[300],
			createdby: [''],
			createdon: [''],
			modifiedby: [''],
			modifiedon: [''],
			summary: [''],
			description: [''],
			queue: [''],
			isactive: [true],
			type: ['Local']
		});

	}
	ngOnInit() {
	}

	public save(controls: FormArray) {
		console.log(controls);

	}
	public cancel() {
		if (this.taskId === '') {

		}
		else {
		}
	}
	ngAfterViewInit() {
	}
}