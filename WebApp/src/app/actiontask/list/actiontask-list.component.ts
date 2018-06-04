import { Component, ViewEncapsulation, OnInit, Inject, ViewChild, ElementRef, AfterViewInit, Input, Output, forwardRef, EventEmitter } from '@angular/core';
import { ActionTaskService } from './../actiontask.service';
import { FormGroup, AbstractControl, FormBuilder, Validators, FormArray } from '@angular/forms';
import { GridOptions } from "ag-grid/main";
import { AppState } from '../../app.service';
import { SnotifyService } from 'ng-snotify';
import { AtionTaskListModel, ActionTaskGridModel } from './../model/actiontaskmodels';
import { GridSearchModel, FilterModel } from './../../common/model/grid-searchmodel';
import { Router } from '@angular/router'
declare const require: any;


@Component({
    selector: 'actiontask-list',
    templateUrl: './actiontask-list.component.html',
    encapsulation: ViewEncapsulation.None,
    styles: [require('./actiontask-list.component.css')],
    providers: [
        ActionTaskService
    ],
})

export class ActionTaskListComponent implements OnInit, AfterViewInit {
    public form: FormGroup;
    private gridOptions: GridOptions;
    public rowData: any[] = [];
    private columnDefs: any[];
    private defaultSizePerPage = 50;
    private pageNumber: number = 1;

    constructor( @Inject(FormBuilder) fb: FormBuilder, public appState: AppState,
        public _actiontaskManager: ActionTaskService, private _notification: SnotifyService,
        private router: Router
    ) {
        this.form = fb.group({
            txtSearch: ['']
        });
        this.gridOptions = <GridOptions>{
            onGridReady: () => {
                this.gridOptions.api.sizeColumnsToFit();
            },
            paginationPageSize: this.defaultSizePerPage,
            enableColResize: true,
            enableServerSideFilter: true,
            enableServerSideSorting: true,
            pagination: true,
            paginationStartPage: 0,
            rowModelType: "pagination",
            suppressCellSelection: true,
            suppressRowSelection: false,
            rowSelection: 'single'
        };
        this.columnDefs = [
            { headerName: "Id", hide: true, field: "Id" },
            { headerName: "Name", field: "Name" },
            { headerName: "Name space", field: "Namespace" },
            { headerName: "Summary", field: "Summary" },
            { headerName: "Is Deleted", hide: true, field: "IsActive" },
            { headerName: "Created By", field: "CreatedBy" },
            { headerName: "Created On", field: "CreatedOn" },
            { headerName: "Modified On", field: "ModifiedOn" },
            { headerName: "Modified By", field: "ModifiedBy" }
        ];
    }

    dataSource = {
        getRows: (params: any) => {
            let searchStr: string = this.form.controls["txtSearch"].value;
            var sortState = this.gridOptions.api.getSortModel();
            let gridSearchModel: GridSearchModel = new GridSearchModel();
            console.log(sortState);
            if (sortState.length !== 0) {
                gridSearchModel.sort = sortState[0].colId + "," + sortState[0].sort;
            }
            if (searchStr) {
                gridSearchModel.quickFilter = searchStr;
            }
            var filterState = this.gridOptions.api.getFilterModel();
            console.log(filterState);
            var filterSend = [];
            for (var k in filterState) {
                if (filterState.hasOwnProperty(k)) {
                    let filter: FilterModel = new FilterModel();
                    filter.column = k;
                    filter.filter = filterState[k].filter;
                    filter.type = filterState[k].type;
                    gridSearchModel.filterPerColumn.push(filter);
                }
            }
            gridSearchModel.page = params.endRow / this.gridOptions.paginationPageSize;
            gridSearchModel.size = this.gridOptions.paginationPageSize;
            this._actiontaskManager.GetActionTasks(gridSearchModel).subscribe(
                result => {
                    var rowsThisPage = result.data;
                    var lastRow = result.totalElements;
                    params.successCallback(rowsThisPage, lastRow);
                });
        }
    }

    ngOnInit() {
    }
    ngAfterViewInit() {
        this.gridOptions.api.setDatasource(this.dataSource);
    }

    public search() {
        this.gridOptions.api.setDatasource(this.dataSource);

    }
    private selectedRow() {
        var selectedRows = this.gridOptions.api.getSelectedRows();
        if (selectedRows && selectedRows.length == 1) {
            var id = selectedRows[0].ActionTaskId;
            var name = selectedRows[0].Name;
            return { id: id, name: name };
        }
        else {
            this._notification.warning('Warning', 'Please select a row');
            return null;
        }
    }

    public edit() {
        var selectedRow = this.selectedRow();
        if (selectedRow) {
            this.router.navigate(['general', { id: selectedRow.id }]);
        }
    }
    public copy() {
        var selectedRow = this.selectedRow();
        if (selectedRow) {
            this.router.navigate(['general', { id: selectedRow.id, isCopy: true }]);
        }
    }
    public newAT() {
        this.router.navigate(['general']);
    }
    public delete() {
        var selectedRow = this.selectedRow();
        if (selectedRow) {
            this._actiontaskManager.DeleteActionTask(selectedRow.id).subscribe(
                result => {
                    this._notification.success('Success', 'Successfully deleted');
                });
        }
    }
}