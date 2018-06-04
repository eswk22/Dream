import { Component, ViewChild,Input } from '@angular/core';


import { AppState } from '../app.service';

import {CompilerResultModel} from './CompilerResultModel';




@Component({
	// The selector is what angular internally uses
	// for `document.querySelectorAll(selector)` in our index.html
	// where, in this case, selector is the string 'home'
	selector: 'compilerresult',  // <home></home>
	// We need to tell Angular's Dependency Injection which providers are in our app.
	providers: [
	],
	// We need to tell Angular's compiler which directives are in our template.
	// Doing so will allow Angular to attach our behavior to an element
	directives: [
		CompilerResult
	],
	// We need to tell Angular's compiler which custom pipes are in our template.
	pipes: [],
	// Our list of styles in our component. We may add more to compose many styles together
	styleUrls: ['./CompilerResult.style.css'],
	// Every Angular template is first compiled by the browser before Angular runs it's compiler
	templateUrl: './CompilerResult.template.html'
})
export class CompilerResult {
	// Set our default values
	
	value: string;
	@Input('Result') Result: CompilerResultModel;
	

	// TypeScript public modifiers
	constructor(public appState: AppState) {
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





}



