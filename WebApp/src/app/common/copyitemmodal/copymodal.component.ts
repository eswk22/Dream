import { Component, ViewEncapsulation, Inject, AfterViewInit } from '@angular/core';
import { FormGroup, AbstractControl, FormBuilder, Validators, FormArray } from '@angular/forms';

import { MatDialogRef, MatDialogConfig, MatDialog, MAT_DIALOG_DATA } from "@angular/material";

declare const require: any;

@Component({
	selector: 'copy-modal',
	templateUrl: './copymodal.component.html',
	encapsulation: ViewEncapsulation.None,
	styles: [require('./copymodal.component.css')],
	providers: [
		
	],
})

export class CopyModalComponent implements AfterViewInit {
	public form: FormGroup;


	constructor(public dialogRef: MatDialogRef<CopyModalComponent>,
		@Inject(FormBuilder) fb: FormBuilder
	) {
		this.form = fb.group({
			txtNamespace: ['', Validators.compose([Validators.required])],
			txtName: ['', Validators.compose([Validators.required])]
		})
	}

	ngAfterViewInit() {
	}




}