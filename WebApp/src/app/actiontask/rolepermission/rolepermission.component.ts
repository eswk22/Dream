import { Component, ViewEncapsulation, OnInit, Inject, ViewChild, ElementRef, AfterViewInit, Input, Output, forwardRef, EventEmitter } from '@angular/core';
//import { CompilerService } from './editor.service';
import { AppState } from '../../app.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { SnotifyService } from 'ng-snotify';
import { GridOptions } from "ag-grid/main";
import { MatDialog } from '@angular/material';
import { RolePermission } from './../model/rolepermissionmodel';
import { RoleModalComponent,RoleModel } from './../../common/rolemodal';

declare const require: any;

@Component({
	selector: 'role-permission',
	templateUrl: './rolepermission.component.html',
	encapsulation: ViewEncapsulation.None,
	styles: [require('./rolepermission.component.css')],
	providers: [
		{
			provide: NG_VALUE_ACCESSOR,
			useExisting: forwardRef(() => RolePermissionComponent),
			multi: true
		}
	],
})

export class RolePermissionComponent implements OnInit, AfterViewInit {
	private gridOptions: GridOptions;
	//public rowData: any[];
	private columnDefs: any[];
	public _value: Array<RolePermission>;

	@Input() ParetId: string;

	@Input() readonly: boolean;

	@Input() set value(v: Array<RolePermission>) {
		if (v !== this._value) {
			this._value = v;
			this.onChange(v);
		}
	}
	get value(): Array<RolePermission> { return this._value; };
	@Output() change = new EventEmitter();


	constructor(public appState: AppState, private _notification: SnotifyService
		, public dialog: MatDialog) {
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
			enableSorting : true
		};
		this.columnDefs = [
			{ headerName: "Role Id", field: "RoleId", hide : true },
			{ headerName: "Role Name", field: "RoleName" },
			{ headerName: "Is Deleted", field: "IsActive", hide: true },
			{ headerName: "View", field: "IsView", editable: true },
			{ headerName: "Edit", field: "IsEdit", editable: true },
			{ headerName: "Execute", field: "IsExecute", editable: true },
			{ headerName: "Admin", field: "IsAdmin", editable: true }
		];		
	}

	private add() {
		let dialogRef = this.dialog.open(RoleModalComponent);
		dialogRef.afterClosed().subscribe(result => {
			if (result) {
				result.forEach(role => {
					let item: RolePermission = new RolePermission();
					item.RoleId = role.Id;
					item.RoleName = role.Name;
					item.IsView = true;
					item.IsActive = true;
					this._value.push(item);
				});
				this.gridOptions.api.setRowData(this._value);
				this.onChange(this._value);
			}
		});
	}

	private remove() {
		
		if (this.gridOptions.api.getSelectedNodes().length === 0) {
			this._notification.success("Error", "Please select a role");
		}
		else {
			this.gridOptions.api.removeItems(this.gridOptions.api.getSelectedNodes());
			//this._value = this.gridOptions.rowData;
			this.onChange(this._value);
		}
	}

	private applyDefault() {
	}

	ngOnInit() {
	}

	ngAfterViewInit() {
	}


	onChange(_) { }
	onTouched() { }
	registerOnChange(fn) { this.onChange = fn; }
	registerOnTouched(fn) { this.onTouched = fn; }

	writeValue(value: Array<RolePermission>) {
		this._value = value || [];
		if (this.gridOptions && this.gridOptions.api) {
			this.gridOptions.api.setRowData(this._value);
		}

	}
}