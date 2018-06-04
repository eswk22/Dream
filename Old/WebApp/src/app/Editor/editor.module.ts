import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgaModule } from '../theme/nga.module';
import { MonacoEditorComponent } from 'ng2-monaco-editor';
import { ErrorPanelComponent } from './error-panel';
// ag-grid
import { AgGridModule } from "ag-grid-angular/main";
import { routing } from './editor.routes';


@NgModule({
    imports: [
		AgGridModule.withComponents([]),
		CommonModule,
        ReactiveFormsModule,
        FormsModule,
        NgaModule,
        routing
    ],
	declarations: [
		ErrorPanelComponent
	],
	exports: [
		ErrorPanelComponent
	]
})
export class EditorModule { }
