import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// ag-grid
import { AgGridModule } from "ag-grid-angular/main";
import { routing } from './process.routes';
import { FlexLayoutModule } from "@angular/flex-layout";
import { MdSelectComponent } from './flowchart/properties/md-select.component';
//import { MaterialModule } from '@angular/material';

import { MdModule } from './../md.module';
import { FlowChartComponent } from './flowchart/flowchart.component';
import { GeneralComponent } from './general/general.component';
import { PropertyComponent } from './flowchart/properties/properties.component';
import { ProcessListComponent } from './processlist/list.component';
import { ParameterComponent } from './../common/parameters/parameters.component';
import { GridButtonComponent } from './../common/parameters/gridbutton.component';
import { ParameterModalComponent } from './../common/parameters/modal/parametermodal.component';

import 'ag-grid/dist/styles/theme-fresh.css'; 

@NgModule({
    imports: [
		AgGridModule.withComponents([]),
		CommonModule,
        ReactiveFormsModule,
		FormsModule,
		MdModule.forRoot(),
		routing,
	//	MaterialModule,
		FlexLayoutModule
    ],
	declarations: [
		FlowChartComponent,
		GeneralComponent,
		PropertyComponent,
		MdSelectComponent,
		ProcessListComponent
	],
	exports: [
		FlowChartComponent,
		GeneralComponent,
		PropertyComponent,
		MdSelectComponent,
		ProcessListComponent
	],
	entryComponents: [PropertyComponent, MdSelectComponent]
})
export class ProcessModule { }
