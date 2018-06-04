import { Component, ViewEncapsulation, OnInit, ViewChild, ElementRef, AfterViewInit, Input, Output, forwardRef, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { SimpleNotificationsModule, NotificationComponent, NotificationsService } from 'angular2-notifications';
import { CompilerArgs, CompilationResult, CompilationResultDiagnostic } from './model/compilerargs';
import { CompilerService } from './editor.service';


declare const monaco: any;
declare const require: any;

@Component({
	selector: 'monaco-editor',
	templateUrl: './editor.component.html',
	encapsulation: ViewEncapsulation.None,
	styles: [require('./editor.component.scss')],
	providers: [
		{
			provide: NG_VALUE_ACCESSOR,
			useExisting: forwardRef(() => EditorComponent),
			multi: true
		},
		CompilerService
	],
})

export class EditorComponent implements OnInit, AfterViewInit {

	@ViewChild('editor') editorContent: ElementRef;
	code: string = 'function x() {\nconsole.log("Hello world!");\n}';
	OptimizationsEnabled: boolean = false;
	language: string = 'csharp';
	targetlanguage: string = this.language;
	language_defaults: any = {
		compilerOptions: {
			noLib: true,
			allowNonTsExtensions: true
		},
		extraLibs: [
			{
				definitions: 'declare class Facts {\n    /**\n    * Returns the next fact\n     */\n    static next():string\n }',
				definitions_name: 'filename/facts.d.ts'
			}
		]
	};
	options: any = {
		lineNumbers: true,
		roundedSelection: false,
		scrollBeyondLastLine: false,
		wrappingColumn: -1,
		folding: false,
		renderLineHighlight: false,
		overviewRulerLanes: 0,
		theme: "vs-dark",
		customPreventCarriageReturn: false,
		glyphMargin : true,
		scrollbar: {
			vertical: 'hidden',
			horizontal: 'auto',
			useShadows: false
		}
	};
	

	public errors: Array<CompilationResultDiagnostic>;
	public warnings: Array<CompilationResultDiagnostic>;
	private _editor: any;
	private _javascriptExtraLibs: any = null;
	private _typescriptExtraLibs: any = null;

	constructor(public compilerService: CompilerService) { }

	
	ngOnInit() {
	}

	ngAfterViewInit() {

		var onGotAmdLoader = () => {
			// Load monaco
			(<any>window).require.config({ paths: { 'vs': 'assets/monaco/vs' } });
			(<any>window).require(['vs/editor/editor.main'], () => {
				this.initMonaco();
			});
		};

		// Load AMD loader if necessary
		if (!(<any>window).require) {
			var loaderScript = document.createElement('script');
			loaderScript.type = 'text/javascript';
			loaderScript.src = 'assets/monaco/vs/loader.js';
			loaderScript.addEventListener('load', onGotAmdLoader);
			document.body.appendChild(loaderScript);
		} else {
			onGotAmdLoader();
		}
	}

	/**
	 * Upon destruction of the component we make sure to dispose both the editor and the extra libs that we might've loaded
	 */
	ngOnDestroy() {
		this._editor.dispose();
		if (this._javascriptExtraLibs !== null) {
			this._javascriptExtraLibs.dispose();
		}

		if (this._typescriptExtraLibs !== null) {
			this._typescriptExtraLibs.dispose();
		}
	}

	// Will be called once monaco library is available
	initMonaco() {
		var myDiv: HTMLDivElement = this.editorContent.nativeElement;
		let options = this.options;
		options.value = this.code;
		options.language = this.language;

		this._editor = monaco.editor.create(myDiv, options);

		// Set language defaults
		// We already set the language on the component so we act accordingly
		if (this.language_defaults !== null) {
			switch (this.language) {
				case 'javascript':
					monaco.languages.typescript.javascriptDefaults.setCompilerOptions(
						this.language_defaults.compilerOptions
					);
					for (var extraLib in this.language_defaults.extraLibs) {
						this._javascriptExtraLibs = monaco.languages.typescript.javascriptDefaults.addExtraLib(
							this.language_defaults.extraLibs[extraLib].definitions,
							this.language_defaults.extraLibs[extraLib].definitions_name
						);
					}
					break;
				case 'typescript':
					monaco.languages.typescript.typescriptDefaults.setCompilerOptions(
						this.language_defaults.compilerOptions
					);
					for (var extraLib in this.language_defaults.extraLibs) {
						this._typescriptExtraLibs = monaco.languages.typescript.typescriptDefaults.addExtraLib(
							this.language_defaults.extraLibs[extraLib].definitions,
							this.language_defaults.extraLibs[extraLib].definitions_name
						);
					}
					break;
			}
		}

		// Currently setting this option prevents the autocomplete selection with the "Enter" key
		// TODO make sure to propagate the event to the autocomplete
		if (this.options.customPreventCarriageReturn === true) {
			let preventCarriageReturn = this._editor.addCommand(monaco.KeyCode.Enter, function () {
				return false;
			});
		}

		this._editor.getModel().onDidChangeContent((e) => {
			this.updateValue(this._editor.getModel().getValue());
		});

		
	}

	/**
	 * UpdateValue
	 *
	 * @param value
	 */
	updateValue(value: string) {
		this.code = value;
		let args: CompilerArgs = new CompilerArgs();
		args.Code = this.code;
		args.OptimizationsEnabled = this.OptimizationsEnabled;
		args.SourceLangage = this.language;
		args.TargetLanguage = this.targetlanguage;
		this.compilerService.CompileCode(args)
			.subscribe(result => {
				this._editor.getModel().deltaDecorations([], this.buildDecorations(result));
			}); 

		
	}

	private buildDecorations(result : CompilationResult) {
		var decorations: any = [];
		var errorOptions = {
			isWholeLine: true,
			className: 'myContentClass',
			glyphMarginClassName: 'myGlyphMarginClass'
		};
		var warningOptions = {
			isWholeLine: true,
			className: 'myContentClass',
			glyphMarginClassName: 'myGlyphMarginClass'
		};
		if (!result.IsSuccess) {
			this.errors = result.Errors;
			this.warnings = result.Warnings;
			for (let item of result.Errors) {
				var glyph: any = {};
				glyph.range = new monaco.Range(item.Start.Line, item.Start.Column, item.End.Line, item.End.Column);
				glyph.options = errorOptions;
				decorations.push(glyph);
			}
			for (let item of result.Warnings) {
				var glyph: any = null;
				glyph.range = new monaco.Range(item.Start.Line, item.Start.Column, item.End.Line, item.End.Column);
				glyph.options = errorOptions;
				decorations.push(glyph);
			}
			return decorations;
		}
		
		
		
	}

	

}