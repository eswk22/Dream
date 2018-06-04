import { Component, ViewEncapsulation, Inject, AfterViewInit } from '@angular/core';
import { FormGroup, AbstractControl, FormBuilder, Validators, FormArray } from '@angular/forms';
import { MdSelectComponent } from './md-select.component';
import { MatDialogRef, MatDialogConfig, MAT_DIALOG_DATA } from "@angular/material";
import { GridOptions } from "ag-grid/main";
import { PropertyModel } from './../../model/propertymodel';
import { SnotifyService } from 'ng-snotify';

declare const require: any;

@Component({
	selector: 'property-modal',
	templateUrl: './properties.component.html',
	encapsulation: ViewEncapsulation.None,
	styles: [require('./properties.component.css')]
})

export class PropertyComponent implements AfterViewInit {
	private ipgridOptions: GridOptions;
	private opgridOptions: GridOptions;
	private ipcolumnDefs: any[];
	private opcolumnDefs: any[];
	private modalData: PropertyModel;
	private inputParams: any[];
	private outputParams: any[];

	constructor(public dialogRef: MatDialogRef<PropertyComponent>,
		@Inject(FormBuilder) fb: FormBuilder, private _notification: SnotifyService,
		@Inject(MAT_DIALOG_DATA) public property: any
	) {
		this.ipgridOptions = <GridOptions>{
			onGridReady: () => {
				this.ipgridOptions.api.sizeColumnsToFit();
			},
			enableColResize: true,
			rowSelection: 'single',
			enableFilter: true,
			enableSorting: true
		};
		this.opgridOptions = <GridOptions>{
			onGridReady: () => {
				this.opgridOptions.api.sizeColumnsToFit();
			},
			enableColResize: true,
			rowSelection: 'single',
			enableFilter: true,
			enableSorting: true
		};
		this.ipcolumnDefs = [
			{ headerName: "Name", field: "name", editable: true },
			{
				headerName: "Target", field: "target", editable: true,
				cellEditorFramework: MdSelectComponent,
				cellEditorParams: {
					items: ['Constants', 'Global', 'Process']
				},
			},
			{ headerName: "Value", field: "paramvalue", editable: true }
		];
		this.opcolumnDefs = [
			{ headerName: "Name", field: "name", editable: true },
			{
				headerName: "Target", field: "target", editable: true,
				cellEditorFramework: MdSelectComponent,
				cellEditorParams: {
					items: ['Constants', 'Global', 'Process']
				},
			},
			{ headerName: "Value", field: "paramvalue", editable: true }
		];

		if (property) {
			console.log(property);
			//ag grid not supporting bi-directional binding.
			this.inputParams = property.input;
			this.outputParams = property.output;
			property.input = [];
			property.output = [];
			this.modalData = property;
		}
	}
	private onModelUpdated() {
		console.log('onModelUpdated');
		this.modalData.input = this.ipgridOptions.rowData;
		this.modalData.output = this.opgridOptions.rowData;
	}

	private addOutput() {
		this.opgridOptions.api.addItems([{}]);
	}
	private removeOutput() {
		if (this.opgridOptions.api.getSelectedNodes().length === 0) {
			this._notification.success("Error", "Please select a parameter");
		}
		else {
			this.opgridOptions.api.removeItems(this.opgridOptions.api.getSelectedNodes());
		}
	}
	private addInput() {
		this.ipgridOptions.api.addItems([{}]);
	}
	private removeInput() {
		if (this.ipgridOptions.api.getSelectedNodes().length === 0) {
			this._notification.success("Error", "Please select a parameter");
		}
		else {
			this.ipgridOptions.api.removeItems(this.ipgridOptions.api.getSelectedNodes());
		}
	}
	
	ngAfterViewInit() {
	}




}