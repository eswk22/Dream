import { Component, ViewChild } from '@angular/core';

import { AppState } from '../app.service';

import {EditorDirective} from './editor.directive';

import { CompilationArguments } from './process';


import { CompilerService } from './compiler.service';

import {CompilerResult,CompilerResultModel} from '../compilerResult';



@Component({
	// The selector is what angular internally uses
	// for `document.querySelectorAll(selector)` in our index.html
	// where, in this case, selector is the string 'home'
	selector: 'codeEditor',  // <home></home>
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
export class editor {
	// Set our default values
	value: string;
	@ViewChild(EditorDirective)
	
	editorDirective: EditorDirective;
	Result: CompilerResultModel;
	compileargs: CompilationArguments;
	selectedLanguage : string;
	languages = [{ text: 'CSharp' }, { text: 'VB' }, { text: 'CSharpScript' }, { text: 'VBScript' }];
	

	//  localState = { value: 'Eswar' };
	// TypeScript public modifiers
	constructor(public appState: AppState, private compilerService: CompilerService) {
		//	  this.localState.value = 'public void';
		this.Result = {
			isSuccess: true,
			decompiledResults: '',
			errors: [],
			warnings: []
		};

	}

	onvaluechange(event) {
		console.log(event);
	}

	ngOnInit() {
		console.log('hello `Home` component');
		// this.title.getData().subscribe(data => this.data = data);
	}


	Onselectlanguage(selectedlanguage: string) {
		this.selectedLanguage = selectedlanguage;
	}

	submitState(value) {
		debugger;
		this.compileargs = {
			code: this.editorDirective.value,
			language: this.selectedLanguage,
			mode: "Script",
			targetlanguage: 'CSharp',
			optimizations: false
		};

		this.compilerService.compile(this.compileargs)
			.subscribe(m => this.Result = m,
					   err => console.log(err));

		console.log('submitState', this.editorDirective.value);
		this.appState.set('value', value);
		//  this.localState.value = '';
	}

}



