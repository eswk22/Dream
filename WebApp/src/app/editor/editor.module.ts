import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MonacoEditorComponent } from 'ng2-monaco-editor';
// ag-grid
import { AgGridModule } from "ag-grid-angular/main";
import { routing } from './editor.routes';
import { EditorComponent } from './editor.component';
@NgModule({
    imports: [
		AgGridModule.withComponents([]),
		CommonModule,
        ReactiveFormsModule,
        FormsModule,
        routing
    ],
	declarations: [
		EditorComponent
	], 
	exports: [
		EditorComponent
	],
	entryComponents: [EditorComponent]
})
export class EditorModule { }
