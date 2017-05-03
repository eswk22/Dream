import { Component, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AppState } from '../app.service';
import { ActionTasklistService } from './actiontasklist.service';
import {ActionTasklistModel} from './actionTasklistModel';
import {AgGridNg2} from 'ag-grid-ng2/main';
import {GridOptions} from 'ag-grid/main';
import { Router, ROUTER_DIRECTIVES } from '@angular/router';


@Component({
    // The selector is what angular internally uses
    // for `document.querySelectorAll(selector)` in our index.html
    // where, in this case, selector is the string 'home'
    selector: 'actiontask',  // <home></home>
    // We need to tell Angular's Dependency Injection which providers are in our app.
    providers: [ActionTasklistService, ActionTasklistModel
    ],
    // We need to tell Angular's compiler which directives are in our template.
    // Doing so will allow Angular to attach our behavior to an element
    directives: [
        actionTasklist, AgGridNg2, ROUTER_DIRECTIVES
    ],
    // We need to tell Angular's compiler which custom pipes are in our template.
    pipes: [],
    // Our list of styles in our component. We may add more to compose many styles together
    styleUrls: ['./actiontasklist.style.css'],
    // Every Angular template is first compiled by the browser before Angular runs it's compiler
    templateUrl: './actiontasklist.template.html'
})
export class actionTasklist {
    private gridOptions: GridOptions;
    private showGrid: boolean;
    private rowCount: string;
    private columnDefs: any[];
    private gridData: ActionTasklistModel[];




    actiontaskForm: FormGroup;

    // TypeScript public modifiers
    constructor(public appState: AppState, private actionTasklistService: ActionTasklistService,
        private formBuilder: FormBuilder, private router: Router) {

    }


   

	onRowSelected($event) {
		var data = $event.node.data;
		if (data != null) {
				this.router.navigate(['/actiontask', data.ActionId]);
            console.log(data.ActionId);
        }
	}
    onvaluechange(event) {
        console.log(event);
    }

    ngOnInit() {
        this.gridOptions = <GridOptions>{
            rowSelection: 'single',
            //       onSelectionChanged: this.onSelectionChanged
        };
        this.createColumnDefs();
        this.showGrid = true;
        this.actionTasklistService.getAll()
            .subscribe(m => this.gridData = m,
            err => console.log(err));
    }


    createColumnDefs() {
        this.columnDefs = [
            { headerName: "ActionId", field: "ActionId", width: 120 },
            { headerName: "Name", field: "key", width: 120 },
            { headerName: "Created By", field: "CreatedBy", width: 450 },
            { headerName: "Created On", field: "CreatedOn", width: 450 },
            { headerName: "Is Active", field: "IsActive", width: 120 }
        ]
    }








}
