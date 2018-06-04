import { Component, ViewEncapsulation, Inject, AfterViewInit } from '@angular/core';
import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { FormGroup, AbstractControl, FormBuilder, Validators, FormArray } from '@angular/forms';
import { AppState } from '../../../app.service';
import { ParameterModel } from './../../model/parametermodel';

import { MatDialogRef, MatDialog, MatDialogConfig, MAT_DIALOG_DATA } from "@angular/material";

declare const require: any;

@Component({
	selector: 'parameter-modal',
	templateUrl: './parametermodal.component.html',
	encapsulation: ViewEncapsulation.None,
	styles: [require('./parametermodal.component.css')],
	//providers: [
	//	//		CompilerService
	//],
})

export class ParameterModalComponent implements AfterViewInit {
	public form: FormGroup;
	//public SavedData: ParameterModel;
	//private ParentId: string;
	//private ParameterType: string;
	//private ParamId: string;


	constructor(public dialogRef: MatDialogRef<ParameterModalComponent>,
		@Inject(MAT_DIALOG_DATA) public selectedData: any,
		@Inject(FormBuilder) fb: FormBuilder) {
		this.form = fb.group({
			name: ['', Validators.compose([Validators.required, Validators.minLength(4)])],
			type: [''],
			defaultvalue: ['']
		});
		if (this.selectedData) {
			this.form.controls["name"].setValue(this.selectedData.Name);
			this.form.controls["type"].setValue(this.selectedData.Type);
			this.form.controls["defaultvalue"].setValue(this.selectedData.DefaultValue);
		}
	}

	ngAfterViewInit() {
	}

	//public save(controls: FormGroup) {
	//	console.log(controls);
	//	return this.ReadData(controls);
	//}
	//public cancel() {
	//	if (this.ParamId === '' || this.ParamId === undefined) {
	//		this.ClearData();
	//	}
	//	else {
	//		this.LoadData(this.SavedData);
	//	}
	//}

	//public ClearData() {
	//	this.form.controls["name"].setValue('');
	//	this.form.controls["type"].setValue('');
	//	this.form.controls["defaultvalue"].setValue('');
	//}


	//public LoadData(data: ParameterModel) {
	//	this.form.controls["name"].setValue(data.Name);
	//	this.form.controls["type"].setValue(data.Type);
	//	this.form.controls["defaultvalue"].setValue(data.DefaultValue);
	//	this.ParamId = data.Id;
	//	this.ParameterType = this.ParameterType;
	//	this.ParentId = data.ParentId;
	//}


}