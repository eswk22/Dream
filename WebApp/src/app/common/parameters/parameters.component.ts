import { Component, ViewEncapsulation, OnInit, Inject, ViewChild, ElementRef, AfterViewInit, Input, Output, forwardRef, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { FormGroup } from '@angular/forms';
import { ParameterModel } from './../model/parametermodel';
import { SnotifyService } from 'ng-snotify';
import { MatDialog } from '@angular/material';

//import { CompilerService } from './editor.service';
import { AppState } from '../../app.service';

import { GridOptions } from "ag-grid/main";

import { ParameterModalComponent } from './modal/parametermodal.component';
import { GridButtonComponent } from './gridbutton.component';

declare const monaco: any;
declare const require: any;

@Component({
	selector: 'parameter-view',
	templateUrl: './parameters.component.html',
	encapsulation: ViewEncapsulation.None,
	styles: [require('./parameters.component.css')],
	providers: [
		{
			provide: NG_VALUE_ACCESSOR,
			useExisting: forwardRef(() => ParameterComponent),
			multi: true
		},
		ParameterModalComponent
	],
})

export class ParameterComponent implements OnInit, AfterViewInit {
	private gridOptions: GridOptions;
	private ParentId: string;
	private ParameterType: string;
	private ParamId: string;
	private rowNum: number = -1;


	public _value: Array<ParameterModel>;
	private columnDefs: any[];
	@Input() ActiontaskId: string;
	@Input() readonly: boolean;

	@Input() set value(v: Array<ParameterModel>) {
		if (v !== this._value) {
			this._value = v;
			this.onChange(v);
		}
	}
	get value(): Array<ParameterModel> { return this._value; };
	@Output() change = new EventEmitter();

	constructor(public appState: AppState, public dialog: MatDialog
		, private _notification: SnotifyService	) {
		this.gridOptions = <GridOptions>{
			context: {
				componentParent: this
			},
			onGridReady: () => {
				this.gridOptions.api.sizeColumnsToFit();
			},
			enableColResize: true,
			rowSelection: 'single'
		};
		this.columnDefs = [
			{ headerName: "", hide: !this.readonly,cellRendererFramework: GridButtonComponent },
			{ headerName: "Id", hide:true,field: "Id" },
			{ headerName: "Name", field: "Name" },
			{ headerName: "ParentType", field: "ParentType" },
			{ headerName: "ParentId", hide: true, field: "ParentId" },
			{ headerName: "Type", field: "Type" },
			{ headerName: "ParameterType", field: "ParameterType" },
			{ headerName: "Default Value", field: "DefaultValue" }
		];
		this._value = [];
	}

	public OnGridButtonClicked(rownum: number, data: ParameterModel) {
		this.ParamId = data.Id;
		this.ParentId = data.ParentId;
		this.ParameterType = data.ParameterType;
		this.rowNum = rownum;
		this.openParameterDialog(this.ParameterType,data);
	}

	ngOnInit() {
	}

	ngAfterViewInit() {
	}

	remove() {
		if (this.gridOptions.api.getSelectedNodes().length === 0) {
			this._notification.success("Error", "Please select a parameter");
		}
		else {
			this.gridOptions.api.removeItems(this.gridOptions.api.getSelectedNodes());
			this.onChange(this._value);
		}

	}

	openParameterDialog(paramType: string, selectedData: ParameterModel) {
		this.ParameterType = paramType;
		let dialogRef = this.dialog.open(ParameterModalComponent, { data: selectedData });
		dialogRef.afterClosed().subscribe(result => {
			if (result) {
				this.updateGrid(result);
			}
		});
	}

	public updateGrid(form: FormGroup) {
		let data: ParameterModel = new ParameterModel();
		data.Id = this.ParamId === undefined ? null : this.ParamId;
		data.Name = form.controls["name"].value;
		data.Type = form.controls["type"].value;
		data.IsActive = true;
		data.DefaultValue = form.controls["defaultvalue"].value;
		data.ParameterType = this.ParameterType;
		data.ParentId = this.ParentId;
		if (this.rowNum === -1) {
			this._value.push(data);
			this.gridOptions.api.addItems([data]);
		}
		else {
			this._value[this.rowNum] = data;
			this.gridOptions.rowData[this.rowNum] = data;
			this.rowNum = -1;
			this.ParamId = null;
			this.gridOptions.api.setRowData(this._value);
		}
		this.onChange(this._value);
	}

	onChange(_) { }
	onTouched() { }
	registerOnChange(fn) { this.onChange = fn; }
	registerOnTouched(fn) { this.onTouched = fn; }

	writeValue(value: Array<ParameterModel>) {
		this._value = value || [];
		if (this.gridOptions && this.gridOptions.api) {
			this.gridOptions.api.setRowData(this._value);
		}
		
	}

}