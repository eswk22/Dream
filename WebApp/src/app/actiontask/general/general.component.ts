import { Component, ViewEncapsulation, OnInit, Inject, ViewChild, ElementRef, AfterViewInit, Input, Output, forwardRef, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, AbstractControl, FormBuilder, Validators, FormArray } from '@angular/forms';
import { ActionTaskService } from './../actiontask.service';
import { ActionTaskModel } from './../model/actiontaskmodels';
import { RolePermission } from './../model/rolepermissionmodel';
import { ParameterModel } from './../../common/model/parametermodel';
import { ExecuteModalComponent } from './../../common/executemodal/executemodal.component';
import { SnotifyService } from 'ng-snotify';

import { CommitModalComponent } from './../../common/commitmodal/commitmodal.component';
import { AppState } from '../../app.service';

import { MatDialog } from '@angular/material';

import { EditorModule, EditorComponent } from './../../editor';

import { GridOptions } from "ag-grid/main";


declare const require: any;

@Component({
	selector: 'actiontask-general',
	templateUrl: './general.component.html',
	encapsulation: ViewEncapsulation.None,
	styles: [require('./general.component.css')],
	providers: [
		ActionTaskService,
		EditorComponent
	],
})

export class GeneralComponent implements OnInit, AfterViewInit {
	public form: FormGroup;
	private defaultlanguage: string = 'CSharpScript';
	@Input() ActiontaskId: string;
	public SavedData: ActionTaskModel;
	private remotelanguage: string = this.defaultlanguage;
	private locallanguage: string = this.defaultlanguage;
	public remotelanguages: any;
	public locallanguages: any;
	private remoteCode: string;
	private localCode: string;
	private permittedRoles: Array<RolePermission>;
	private parameters: Array<ParameterModel>;
	private codeType: string = 'Local';
	private name: string;
	private readOnly: boolean = false;
	private CreatedBy: string;
	private CreatedOn: Date;
	private ModifiedBy: string;
	private ModifedOn: Date;
	private status: string = 'Draft';

	constructor( @Inject(FormBuilder) fb: FormBuilder, public appState: AppState,
		public _actiontaskManager: ActionTaskService, private _notification: SnotifyService,
		public router: Router, public dialog: MatDialog, private route: ActivatedRoute) {
		this.form = fb.group({
			name: ['', Validators.compose([Validators.required, Validators.minLength(4)])],
			namespace: ['', Validators.compose([Validators.required, Validators.minLength(4)])],
			menupath: ['', Validators.compose([Validators.required, Validators.minLength(4)])],
			timeout: [300],
			summary: [''],
			description: [''],
			queue: [''],
			isactive: [true],
			type: ['Local'],
			permittedRole: []
		});
		this.remotelanguages = [
			{ value: "CSharpScript", DisplayName: "C# - script" },
			{ value: "VBScript", DisplayName: "VB - script" }
		];
		this.locallanguages = [
			{ value: "CSharpScript", DisplayName: "C# - script" },
			{ value: "VBScript", DisplayName: "VB - script" }
		];

	}
    ngOnInit() {
        if (this.route.snapshot.url.length > 0 && this.route.snapshot.url[0].parameters) {
            this.ActiontaskId = this.route.snapshot.url[0].parameters['id'];
        }
        
	}

	public save() {
		let data: ActionTaskModel = this.ReadData(this.form);
		data.Status = "Draft";
		this._actiontaskManager.SaveActionTask(data)
			.subscribe(result => {
				this.SavedData = result;
				this.LoadData(result);
				this._notification.success("Status", "Saved successfully");
			});
	}

	public run() {

		let dialogRef = this.dialog.open(ExecuteModalComponent, { data: this.parameters });
		dialogRef.afterClosed().subscribe(result => {
			if (result) {
			}
		});
	}
	public cancel() {
		console.log(this.SavedData);
		if (this.ActiontaskId === '' || this.ActiontaskId === undefined) {
			this.ClearData();
		}
		else {
			this.LoadData(this.SavedData);
		}
	}


	public edit() {
		let name : string = this.form.controls["name"].value;
		let namespace : string = this.form.controls["namespace"].value;
		this._actiontaskManager.EditActionTask(this.ActiontaskId,name,namespace)
			.subscribe(result => {
				this.SavedData = result;
				this.LoadData(result);	
			});
	}
	public publish() {
		let dialogRef = this.dialog.open(CommitModalComponent);
		dialogRef.afterClosed().subscribe(comment => {
			if (comment) {
				console.log(comment);
				let data: ActionTaskModel = this.ReadData(this.form);
				this._actiontaskManager.PublishActionTask(data, comment)
					.subscribe(result => {
						this.SavedData = result;
						this.LoadData(result);
						this._notification.success("Status", "Successfully published");
					});
			}
		});
	}

	public LoadData(data: ActionTaskModel) {
		this.form.controls["name"].setValue(data.Name);
		this.form.controls["namespace"].setValue(data.Namespace);
		this.form.controls["menupath"].setValue(data.FolderPath);
		this.form.controls["timeout"].setValue(data.Timeout);
		this.form.controls["summary"].setValue(data.Summary);
		this.form.controls["description"].setValue(data.Description);
		this.form.controls["queue"].setValue(data.Queuename);
		this.form.controls["isactive"].setValue(data.IsActive);
		this.form.controls["type"].setValue(data.Type);
		this.CreatedBy = data.CreatedBy;
		this.CreatedOn = data.CreatedOn;
		this.ModifedOn = data.ModifiedOn;
		this.ModifiedBy = data.ModifiedBy;
		this.ActiontaskId = data.ActionTaskId;
		this.remoteCode = data.RemoteCode;
		this.remotelanguage = data.RemoteLanguage;
		this.localCode = data.LocalCode;
		this.locallanguage = data.LocalLanguage;
		this.permittedRoles = data.PermittedRoles;
		this.parameters = data.Parameters;
		this.status = data.Status;
		this.readOnly = data.Status != "Draft";
	}

	public ReadData(form: FormGroup): ActionTaskModel {
		let data: ActionTaskModel = new ActionTaskModel();
		data.ActionTaskId = this.ActiontaskId === undefined ? null : this.ActiontaskId;
		data.Description = form.controls["description"].value;
		data.FolderPath = form.controls["menupath"].value;
		data.IsActive = form.controls["isactive"].value;
		//data.IsLatestVersion = form.controls["modifiedby"].value;

		data.Name = form.controls["name"].value;
		data.Namespace = form.controls["namespace"].value;
		data.Queuename = form.controls["queue"].value;
		data.Summary = form.controls["summary"].value;
		data.Timeout = form.controls["timeout"].value;
		data.Type = form.controls["type"].value;
		data.ModifiedBy = this.ModifiedBy;
		data.ModifiedOn = this.ModifedOn;
		data.CreatedBy = this.CreatedBy;
		data.CreatedOn = this.CreatedOn;
		data.RemoteCode = this.remoteCode;
		data.RemoteLanguage = this.remotelanguage;
		data.LocalCode = this.localCode;
		data.LocalLanguage = this.locallanguage;
		data.PermittedRoles = this.permittedRoles;
		data.Parameters = this.parameters;
		data.Status = this.status;
		//data.Version = form.controls[""].value;
		return data;
	}

	public ClearData() {
		this.form.controls["name"].setValue('');
		this.form.controls["namespace"].setValue('');
		this.form.controls["menupath"].setValue('');
		this.form.controls["timeout"].setValue('');
		this.form.controls["summary"].setValue('');
		this.form.controls["description"].setValue('');
		this.form.controls["queue"].setValue('');
		this.form.controls["isactive"].setValue(true);
		this.form.controls["type"].setValue('Local');
		this.CreatedBy = '';
		this.CreatedOn = null;
		this.ModifiedBy = '';
		this.ModifedOn = null;
		this.remoteCode = '';
		this.remotelanguage = this.defaultlanguage;
		this.localCode = '';
		this.locallanguage = this.defaultlanguage;
		this.permittedRoles = [];
		this.parameters = [];
		this.status = "Draft";
		this.form.markAsUntouched();
	}
	ngAfterViewInit() {
		console.log(this.ActiontaskId);
		if (this.ActiontaskId) {
			this._actiontaskManager.GetActionTask(this.ActiontaskId)
                .subscribe(result => {
                    if (this.route.snapshot.url.length > 0 && this.route.snapshot.url[0].parameters) {
                        var isCopy = this.route.snapshot.url[0].parameters['isCopy'];
                        if (isCopy && isCopy == 'true') {
                            result.ActionTaskId = '';
                            result.Name = '';
                            result.Status = 'Draft';
                        }
                    }

					this.LoadData(result);
					this.SavedData = result;
				});
		}

	}
}