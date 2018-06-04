import { Component, ViewEncapsulation, Inject, AfterViewInit } from '@angular/core';

import { MatDialogRef, MatDialogConfig, MAT_DIALOG_DATA } from "@angular/material";
import { GridOptions } from "ag-grid/main";
import { RoleModel } from "./model/rolemodel";
import { RoleModalService } from './rolemodal.service';

declare const require: any;

@Component({
	selector: 'role-modal',
	templateUrl: './rolemodal.component.html',
	encapsulation: ViewEncapsulation.None,
	styles: [require('./rolemodal.component.css')],
	providers: [
		RoleModalService
	],
})

export class RoleModalComponent implements AfterViewInit {
	private gridOptions: GridOptions;
	public rowData: Array<RoleModel>;
	private selectedRows: Array<RoleModel>;
	private columnDefs: any[];


	constructor(public _roleManager: RoleModalService, public dialogRef: MatDialogRef<RoleModalComponent>
	) {
		this.gridOptions = <GridOptions>{
			context: {
				componentParent: this
			},
			onGridReady: () => {
				this.gridOptions.api.sizeColumnsToFit();
			},
			//defaultColDef: {
			//	checkboxSelection: function (params) {
			//		var isGrouping = false;
			//		if (this.gridOptions) {
			//			var isGrouping = this.gridOptions.columnApi.getRowGroupColumns().length > 0;
			//		}
			//		return params.colIndex === 0 && !isGrouping;
			//	}
			//},
			enableColResize: true,
			suppressCellSelection: true,
			suppressRowClickSelection: false,
			rowSelection: 'multiple',
			groupSelectsChildren: true,
			groupDefaultExpanded: -1,
			groupSuppressAutoColumn: true
		};
		this.columnDefs = [
			{
				headerName: '',
				checkboxSelection: true,
				suppressSorting: true,

				suppressMenu: true,
				pinned: true
			},
			{
				headerName: "Id",
				field: "Id",
				hide: true
			},
			{
				headerName: "Name",
				field: "Name"
			}
		];
		this.rowData = [{
			Id: 'dofjoisdjaiojfs', Name: 'Developer'
		}, {
			Id: 'jsdhfosdjfsdjf', Name: 'Admin'
		}];
	}

	ngAfterViewInit() {
		this._roleManager.GetRoles()
			.subscribe(result => {
				if (result) {
					this.gridOptions.api.addItems(result);
				}
			});
	}
	onSelectionChanged(event: any) {
		this.selectedRows = this.gridOptions.api.getSelectedRows();
	}




}