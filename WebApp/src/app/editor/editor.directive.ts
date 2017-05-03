/// <reference path="../../../typings/index.d.ts" />

import { Component, Directive, ElementRef, Input, Output, EventEmitter } from '@angular/core';
import {editorOptions} from './editorOptions';



/*
 * Directive
 * XLarge is a simple directive to show how one is made
 */



function buildGetAnnotations(data) {
    return (cm, updateLinting) => {
        data.lint(data.value, updateLinting);
    };
}



@Directive({
    selector: '[codeEditor]' // using [ ] means selecting attributes

})
export class EditorDirective {
    // codeEditor: CodeMirror.Editor;
    private textarea: HTMLElement;
    private instance: CodeMirror.Editor;
    private lintOptions: CodeMirror.LintOptions;
    private lintStateOptions: CodeMirror.LintStateOptions;
    annotations: CodeMirror.Annotation[];
    updateLintingCallback: CodeMirror.UpdateLintingCallback;
    @Input('options') options: Object;
    @Input('mode') mode: string;
    @Input('lint') lint: Function;
    @Output() valueChange = new EventEmitter();



    constructor(el: ElementRef) {
        this.textarea = el.nativeElement;
    }

    @Input() set value(value: string) {
        if (this.instance != null) {
            if (this.instance.getDoc().getValue() === value)
                return;
            this.instance.getDoc().setValue(value);
        }

    }

    get value(): string {
        if (this.instance == null)
            return;
        return this.instance.getDoc().getValue();
    }




    ngOnInit() {
        const options1 = Object.assign(
            {},
            this.options,
            this.mode !== undefined ? { mode: this.mode } : {}
            //this.lint !== undefined ? {
            //             lint: { async: true, getAnnotations: buildGetAnnotations(this) }
            //         } : {}
        );
        this.instance = CodeMirror.fromTextArea(this.textarea, options1);
        this.instance.setOption('lineNumbers', true);
        this.instance.setOption('lint', true);
        //this.instance.setOption('gutters', ['CodeMirror-linenumbers', 'CodeMirror-lint-markers']);

        this.lintStateOptions = {
            async: true,
            hasGutters: true
        };

        this.annotations = [{
            from: {
                ch: 0,
                line: 0
            },
            to: CodeMirror.Pos(1, 0),
            message: "test",
            severity: "warning"
        }];

        this.lintOptions = {
            async: true,
            hasGutters: true,
            getAnnotations: (content: string,
                updateLintingCallback: CodeMirror.UpdateLintingCallback,
                options: CodeMirror.LintStateOptions,
                codeMirror: CodeMirror.Editor) => {
                console.log(content);
            }
        };



        this.updateLintingCallback = (codeMirror: CodeMirror.Editor,
            annotations: CodeMirror.Annotation[]) => {
        };

        this.instance.on("change", (editor: CodeMirror.Editor) => {
			 //this.valueChange.emit({
    //            value: this.value
				//});
		   
        });
 

    }

   



}
