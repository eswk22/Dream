import { Component, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AppState } from '../app.service';
import { EditorDirective, editor, CompilationArguments, CompilerService} from '../editor';
import { ActionTaskService } from './actiontask.service';
import { ActionTaskModel} from './actionTaskModel';
import { AgGridNg2} from 'ag-grid-ng2/main';
import { GridOptions} from 'ag-grid/main';
import { Router, ActivatedRoute } from '@angular/router';
import {CompilerResult, CompilerResultModel, errorModel} from '../compilerResult';

@Component({
    // The selector is what angular internally uses
    // for `document.querySelectorAll(selector)` in our index.html
    // where, in this case, selector is the string 'home'
    selector: 'actiontask',  // <home></home>
    // We need to tell Angular's Dependency Injection which providers are in our app.
    providers: [ActionTaskService, ActionTaskModel, CompilerService
    ],
    // We need to tell Angular's compiler which directives are in our template.
    // Doing so will allow Angular to attach our behavior to an element
    directives: [
        actionTask, EditorDirective, AgGridNg2, editor, CompilerResult
    ],
    // We need to tell Angular's compiler which custom pipes are in our template.
    pipes: [],
    // Our list of styles in our component. We may add more to compose many styles together
    styleUrls: ['./actiontask.style.css'],
    // Every Angular template is first compiled by the browser before Angular runs it's compiler
    templateUrl: './actiontask.template.html'
})
export class actionTask implements OnInit, OnDestroy {
    private gridOptions: GridOptions;
    private showGrid: boolean;
    private rowCount: string;
    private columnDefs: any[];
    private InputParam: any[];
    private OutputParam: any[];

	value: string;
	@ViewChild(EditorDirective)
	private accesscode: string;
	editorDirective: EditorDirective;
	Result: CompilerResultModel;
	compileargs: CompilationArguments;


    languages = [{ DisplayName: 'CSharp', Value: 'CSharp' },
        { DisplayName: 'VB', Value: 'VBNet' },
        { DisplayName: 'CSharpScript', Value: 'CSharpScript' },
        { DisplayName: 'VBScript', Value: 'VBNetScript' }];

    actiontypes = [{ DisplayName: 'Access', Value: 'Access' },
        { DisplayName: 'Remote', Value: 'Remote' }]
 
    actiontaskForm: FormGroup;

    // TypeScript public modifiers
    constructor(public appState: AppState, private actionTaskService: ActionTaskService,
        private formBuilder: FormBuilder, private route: ActivatedRoute, private compilerService: CompilerService) {
		this.Result = {
			IsSuccess: true,
			Decompiled: '',
			Errors: [],
			Warnings: [],
			Infos: []
		};
    }

    //onvaluechange(event) {
    //    console.log(event);
    //}

    ngOnInit() {

		this.actiontaskForm = this.formBuilder.group({
            name: ['', Validators.required],
            description: [''],
            summary: [''],
            createdby: [''],
            updatedby: [''],
            createdon: [''],
            updatedon: [''],
            isactive: [true],
            module: ['', Validators.required],
            menupath: ['', Validators.required],
            actiontype: ['', Validators.required],
            timeout: [300, Validators.required],
            codelanguage: ['CSharpScript', Validators.required],
            remotecode: [''],
            accesscode: ['']
        });

        this.route.params.subscribe(params => {
            let id = params['id'];
			this.getById(id);
        });


        //this.actiontaskForm.valueChanges.subscribe(value => {
        //    this.OnFormValueChanges();
        //});

        this.gridOptions = <GridOptions>{};
        this.createColumnDefs();
        this.showGrid = true;
        this.InputParam = [{ key: 'x', value: 10 },
            { key: 'y', value: 5 }];
        this.OutputParam = [];
    }


    createColumnDefs() {
        this.columnDefs = [
            { headerName: "Name", field: "key", width: 120 },
            { headerName: "Default Value", field: "value", width: 450 }
        ]
    }


    OnFormValueChanges() {

    }

	OnCompile(){
	this.compileargs = {
		Code: this.editorDirective.value,
		SourceLanguage: this.actiontaskForm.controls['codelanguage'].value,
		TargetLanguage: this.actiontaskForm.controls['codelanguage'].value,
		OptimizationsEnabled: false
	};

	this.compilerService.compile(this.compileargs)
		.subscribe(m => this.Result = m,
		err => console.log(err));
}

	OnRemoteCodeExecute() {
		debugger;
		let actiontaskmodel: ActionTaskModel = this.translatecontrolvalues(this.actiontaskForm.value);
		console.log(this.accesscode);
		this.actionTaskService.executeCode(actiontaskmodel)
            .subscribe(m => this.assignvaluestocontrol(m),
            err => console.log(err));
	}

	getById = (ActionId: string) => {
		this.actionTaskService.getById(ActionId)
			.subscribe(m => this.assignvaluestocontrol(m),
            err => console.log(err));
	}

    submitState() {
		let actiontaskmodel: ActionTaskModel = this.translatecontrolvalues(this.actiontaskForm.value);
		this.actionTaskService.save(actiontaskmodel)
            .subscribe(m => this.assignvaluestocontrol(m),
            err => console.log(err));
    }

    assignvaluestocontrol = (value: ActionTaskModel) => {
        (<FormControl>this.actiontaskForm.controls['name']).updateValue(value.Name);
        (<FormControl>this.actiontaskForm.controls['summary']).updateValue(value.Summary);
        (<FormControl>this.actiontaskForm.controls['createdby']).updateValue(value.CreatedBy);
        (<FormControl>this.actiontaskForm.controls['updatedby']).updateValue(value.UpdatedBy);
        (<FormControl>this.actiontaskForm.controls['createdon']).updateValue(value.CreatedOn);
        (<FormControl>this.actiontaskForm.controls['isactive']).updateValue(value.IsActive);
        (<FormControl>this.actiontaskForm.controls['menupath']).updateValue(value.menupath);
        (<FormControl>this.actiontaskForm.controls['actiontype']).updateValue(value.Actiontype);
        (<FormControl>this.actiontaskForm.controls['timeout']).updateValue(value.TimeOut);
        (<FormControl>this.actiontaskForm.controls['codelanguage']).updateValue(value.Codelanguage);
		this.editorDirective.value = value.AccessCode;
    //    (<FormControl>this.actiontaskForm.controls['remotecode']).updateValue(value.RemoteCode);
    //    (<FormControl>this.actiontaskForm.controls['accesscode']).updateValue(value.AccessCode);
    }

    translatecontrolvalues = (value: any) => {
		debugger;
        let actiontaskmodel = new ActionTaskModel();
        actiontaskmodel.AccessCode = value.accesscode ? value.accesscode : '';
        actiontaskmodel.Description = value.description ? value.description : '';
        actiontaskmodel.Name = value.name ? value.name : '';
        actiontaskmodel.Summary = value.summary ? value.summary : '';
        actiontaskmodel.CreatedBy = value.createdby ? value.createdby : '';
        actiontaskmodel.CreatedOn = value.createdon ? value.createdon : '';
        actiontaskmodel.UpdatedBy = value.updatedby ? value.updatedby : '';
        actiontaskmodel.IsActive = value.isactive ? value.isactive : '';
        actiontaskmodel.menupath = value.menupath ? value.menupath : '';
        actiontaskmodel.Actiontype = value.actiontype ? value.actiontype : '';
		actiontaskmodel.module = value.module ? value.module : '';
        actiontaskmodel.TimeOut = value.timeout ? value.timeout : '';
        actiontaskmodel.Codelanguage = value.codelanguage ? value.codelanguage : '';
        actiontaskmodel.RemoteCode = value.remotecode ? value.remotecode : '';
        actiontaskmodel.AccessCode = this.editorDirective.value
]
		;
    //    actiontaskmodel.Inputs = this.InputParam;
    //    actiontaskmodel.Outputs = this.OutputParam;
        debugger;
        return actiontaskmodel;
    };

    ngOnDestroy = () => {
        this.sub.unsubscribe();
    }
}



