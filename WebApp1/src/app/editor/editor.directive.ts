import { Component, Directive, ElementRef, Input, Output, EventEmitter } from '@angular/core';
import {editorOptions} from './editorOptions';


import 'codemirror/addon/lint/lint';
import 'codemirror/mode/clike/clike';
import 'codemirror/mode/vb/vb';
import 'codemirror';
/*
 * Directive
 * XLarge is a simple directive to show how one is made
 */

declare var CodeMirror: any;

function buildGetAnnotations(data) {
	return (cm, updateLinting) => {
        data.lint(data.value, updateLinting);
    };
}



@Directive({
	selector: '[codeEditor]' // using [ ] means selecting attributes

})
export class EditorDirective {
	codeEditor: any;
	private textarea: HTMLElement;
	private instance: any;
	@Input('options') options: Object;
	@Input('mode') mode: string;
	@Input('lint') lint: Function;
//	@Output() OnValueChange = new EventEmitter();


	
	constructor(el: ElementRef) {
		this.textarea = el.nativeElement;
	}

	@Input() set value(value: string) {
		if (this.instance != null) {
			if (this.instance.getValue() === value)
                return;
			this.instance.setValue(value);
		}
		//this.OnValueChange.emit({
		//	value: this.value
		//})
	}

	get value(): string {
        if (this.instance == null)
			return;
		return this.instance.getValue();
    }




	ngOnInit() {
		const options = Object.assign(
            {},
            this.options,
            this.mode !== undefined ? { mode: this.mode } : {},
			this.lint !== undefined ? {
                lint: { async: true, getAnnotations: buildGetAnnotations(this) }
            } : {}
        );
		this.instance = CodeMirror.fromTextArea(this.textarea, options);
	}



}
