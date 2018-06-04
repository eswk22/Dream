import { Component, ViewEncapsulation, Inject, AfterViewInit } from '@angular/core';
import { FormGroup, AbstractControl, FormBuilder, Validators, FormArray } from '@angular/forms';

import { MatDialogRef, MatDialogConfig, MatDialog, MAT_DIALOG_DATA } from "@angular/material";
import { GridOptions } from "ag-grid/main";
declare const require: any;

@Component({
	selector: 'execute-modal',
	templateUrl: './executemodal.component.html',
	encapsulation: ViewEncapsulation.None,
	styles: [require('./executemodal.component.css')],
	providers: [
		
	],
})

export class ExecuteModalComponent implements AfterViewInit {
	public form: FormGroup;
	private gridOptions: GridOptions;
	private columnDefs: any[];
	private rowData: any[];

	constructor(public dialogRef: MatDialogRef<ExecuteModalComponent>,
		@Inject(MAT_DIALOG_DATA) public paramData: any,
		@Inject(FormBuilder) fb: FormBuilder
	) {
		this.gridOptions = <GridOptions>{
			onGridReady: () => {
				this.gridOptions.api.sizeColumnsToFit();
			},
			enableColResize: true,
			rowSelection: 'multiple',
			groupSelectsChildren: true,
			groupDefaultExpanded: -1,
			groupSuppressAutoColumn: true,
			enableFilter: true,
			enableSorting: true
		};
		this.columnDefs = [
			{ headerName: "Id", hide: true, field: "Id" },
			{ headerName: "Name", field: "Name" },
			{ headerName: "ParentType", hide: true, field: "ParentType" },
			{ headerName: "ParentId", hide: true, field: "ParentId" },
			{ headerName: "Type", field: "Type" },
			//{ headerName: "ParameterType", field: "ParameterType" },
			{ headerName: "Value", field: "DefaultValue", editable: true }
		];
		this.form = fb.group({
		//	txtNamespace: ['', Validators.compose([Validators.required])],
		//	txtName: ['', Validators.compose([Validators.required])]
		})
		if(paramData)
			this.rowData = paramData;
	}

	ngAfterViewInit() {
	}




}