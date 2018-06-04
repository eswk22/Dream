import { Component, ViewChild, Input } from '@angular/core';

import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';

import { AppState } from '../app.service';

import {EditorDirective} from './editor.directive';

import { CompilationArguments } from './process';


import { CompilerService } from './compiler.service';

import {CompilerResult, CompilerResultModel, errorModel} from '../compilerResult';





@Component({
	// The selector is what angular internally uses
	// for `document.querySelectorAll(selector)` in our index.html
	// where, in this case, selector is the string 'home'
	selector: 'editor',  // <home></home>
	// We need to tell Angular's Dependency Injection which providers are in our app.
	providers: [CompilerService
	],
	// We need to tell Angular's compiler which directives are in our template.
	// Doing so will allow Angular to attach our behavior to an element
	directives: [
		EditorDirective, editor, CompilerResult
	],
	// We need to tell Angular's compiler which custom pipes are in our template.
	pipes: [],
	// Our list of styles in our component. We may add more to compose many styles together
	styleUrls: ['./editor.style.css'],
	// Every Angular template is first compiled by the browser before Angular runs it's compiler
	templateUrl: './editor.template.html'
})
export class editor{
	// Set our default values
	value: string;
	@ViewChild(EditorDirective)


	editorDirective: EditorDirective;
	Result: CompilerResultModel;
	compileargs: CompilationArguments;
	editorForm: FormGroup;
	
	@Input() set code(value: string) {
		this.editorDirective.value = value;

    }

    get code(): string {
         return this.editorDirective.value;
    }


    languages = [{ DisplayName: 'CSharp', Value: 'CSharp' },
        { DisplayName: 'VB', Value: 'VBNet' },
        { DisplayName: 'CSharpScript', Value: 'CSharpScript' },
        { DisplayName: 'VBScript', Value: 'VBNetScript' }];
	
	
	// TypeScript public modifiers
    constructor(public appState: AppState, private compilerService: CompilerService,
        private formBuilder: FormBuilder) {
		this.Result = {
			IsSuccess: true,
			Decompiled: '',
			Errors: [],
			Warnings: [],
			Infos:[]
		};

	}

    onvaluechange(event) {
	//	  this.OnFormValueChanges();
	}

    ngOnInit() {
         
		this.editorForm = this.formBuilder.group({
            editorTextArea: ['', Validators.required],
            drplanguage: ['CSharpScript', Validators.required]
		});
        this.editorForm.valueChanges.subscribe(value => {
            this.OnFormValueChanges();
        });
    }

  

    OnFormValueChanges() {
		
        this.compileargs = {
            Code: this.editorDirective.value,
            SourceLanguage: this.editorForm.controls['drplanguage'].value,
            TargetLanguage: this.editorForm.controls['drplanguage'].value,
            OptimizationsEnabled: false
        };

        this.compilerService.compile(this.compileargs)
            .subscribe(m => this.Result = m,
            err => console.log(err));
    }

    submitState() {
        this.OnFormValueChanges();
	}

}



