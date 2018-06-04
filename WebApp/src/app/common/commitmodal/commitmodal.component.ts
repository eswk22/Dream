import { Component, ViewEncapsulation, Inject, AfterViewInit } from '@angular/core';
import { FormGroup, AbstractControl, FormBuilder, Validators, FormArray } from '@angular/forms';

import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from "@angular/material";
import { GridOptions } from "ag-grid/main";

declare const require: any;

@Component({
	selector: 'commit-modal',
	templateUrl: './commitmodal.component.html',
	encapsulation: ViewEncapsulation.None,
	styles: [require('./commitmodal.component.css')],
	providers: [
		
	],
})

export class CommitModalComponent implements AfterViewInit {
	public form: FormGroup;


    constructor(public dialogRef: MatDialogRef<CommitModalComponent>,
		@Inject(FormBuilder) fb: FormBuilder
	) {
		this.form = fb.group({
			txtComment: ['', Validators.compose([Validators.required])]
		})
	}

	ngAfterViewInit() {
	}




}