import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MonacoEditorComponent } from 'ng2-monaco-editor';

// ag-grid
import { AgGridModule } from "ag-grid-angular/main";
import { routing } from './actiontask.routes';
import { GeneralComponent } from './general/general.component';
import { ParameterComponent } from './../common/parameters/parameters.component';
import { GridButtonComponent } from './../common/parameters/gridbutton.component';
import { ParameterModalComponent } from './../common/parameters/modal/parametermodal.component';
import { RolePermissionComponent } from './rolepermission/rolepermission.component';
import { ATReferenceComponent } from './references/references.component';

import { RoleModalComponent } from './../common/rolemodal';
import { CommitModalComponent } from './../common/commitmodal/commitmodal.component';
import { ExecuteModalComponent } from './../common/executemodal/executemodal.component';


import { TabsModule } from "ng2-tabs";
import { MdModule } from './../md.module';
import { EditorModule, EditorComponent } from './../editor';
import { FlexLayoutModule } from "@angular/flex-layout";

//import { MatDialogModule } from '@angular/material';


import { ActionTaskListComponent } from './list/actiontask-list.component';

import 'ag-grid/dist/styles/theme-fresh.css'; 

@NgModule({
    imports: [
		AgGridModule.withComponents([]),
		CommonModule,
        ReactiveFormsModule,
		FormsModule,
		EditorModule,
		MdModule.forRoot(),
		routing,
		TabsModule,
		//MatDialogModule,
		FlexLayoutModule
    ],
	declarations: [
		GeneralComponent,
		RolePermissionComponent,
		ParameterComponent,
		ParameterModalComponent,
		ActionTaskListComponent,
		ATReferenceComponent,
		GridButtonComponent,
		RoleModalComponent,
		CommitModalComponent,
		ExecuteModalComponent
	],
	exports: [
		GeneralComponent,
		RolePermissionComponent,
		ParameterComponent,
		ParameterModalComponent,
		ActionTaskListComponent,
		ATReferenceComponent,
		GridButtonComponent,
		RoleModalComponent,
		CommitModalComponent,
		ExecuteModalComponent
	],
	entryComponents: [ParameterModalComponent, GridButtonComponent, RoleModalComponent, CommitModalComponent, ExecuteModalComponent]
})
export class ActionTaskModule { }
