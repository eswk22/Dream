import {
    Component,
    OnInit
} from '@angular/core';

import { AppState } from '../../app.service';
import { HieraService } from '../hieras.service';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, AbstractControl, FormBuilder, Validators, FormArray } from '@angular/forms';

import { HieraModel, HieraLevelModel } from '../model/hieramodel';
import { Param2HieraLevelModel, ParamModel } from '../model/parammodel';
import { RoleModel, AccountService } from '../../account';
import { NotificationsService } from 'angular2-notifications';

@Component({
    // The selector is what angular internally uses
    // for `document.querySelectorAll(selector)` in our index.html
    // where, in this case, selector is the string 'home'
    selector: 'paramcreate',  // <home></home>
    // We need to tell Angular's Dependency Injection which providers are in our app.
    providers: [
        HieraService, AccountService
    ],
    // Our list of styles in our component. We may add more to compose many styles together
    //styleUrls: ['./hieras.component.css'],
    // Every Angular template is first compiled by the browser before Angular runs it's compiler
    templateUrl: './paramcreate.html'
})
export class ParamCreate implements OnInit {
    // Set our default values
    public localState = { value: '' };
    public form: FormGroup;
    public hieraList: FormArray;
    public parametername: AbstractControl;
    public parametertype: AbstractControl;
    public IsActive: AbstractControl;
    public hieraArray: Array<HieraLevelModel>
    public allRoles: Array<any>;
    public ParameterId: Number = 0;
    private CreatedBy: string;
    private CreatedOn: Date;
    private ModifiedBy: string;
    private ModifiedOn: Date;
    hieraname: string = "Default";
    // TypeScript public modifiers
    constructor(
        public fb: FormBuilder,
        public router: Router,
        public appState: AppState,
        public hieraManager: HieraService,
        public accountManager: AccountService,
        private _notification: NotificationsService,
        private route: ActivatedRoute
    ) {
        this.form = fb.group({
            parametername: ['', Validators.compose([Validators.required, Validators.minLength(4)])],
            parametertype: [''],
            isActive: [true],
            hieraList: this.fb.array([
                this.initHieralevel()
            ]),
            ChildList: this.fb.array([
                //           this.initChild(0)
            ])
        });

        this.parametername = this.form.controls['parametername'];
        this.parametertype = this.form.controls['parametertype'];
        this.IsActive = this.form.controls['isActive'];
        this.parametertype.setValue('Hash');
    }



    private initHieralevel() {
        return this.fb.group({
            hieraParamId: [''],
            hieralevel: [''],
            defaultvalue: [''],
            isMandatory: [false],
            roles: ['']
        });
    }

    private AddHiera() {
        const control = <FormArray>this.form.controls['hieraList'];
        control.push(this.initHieralevel());
    }
    private removeHieralevel(i: number) {
        // remove address from the list
        const control = <FormArray>this.form.controls['hieraList'];
        control.removeAt(i);
    }

    public ngOnInit() {
        this.hieraManager.getHiera(this.hieraname, 1.6)
            .subscribe(result => {
                this.hieraArray = result.HieraLevels;
            });

        this.accountManager.getAllRoles()
            .subscribe(result => {
                if (result) {
                    this.allRoles = new Array<any>();
                    result.forEach(item => {
                        this.allRoles.push({ id: item.Id, text: item.Name });
                    });
                }
            });
        this.ParameterId = this.route.snapshot.queryParams['ParameterId']
        if (this.ParameterId && this.ParameterId != 0) {
            this.hieraManager.getParameter(this.ParameterId)
                .subscribe(result => {
                    this.setParameterValue(result);
                });
            this.hieraManager.getParameters(this.ParameterId)
                .subscribe(result => {
                    this.setChildParamValue(result);

                });
            this.hieraManager.getParam2Hiera(this.ParameterId)
                .subscribe(result => {
                    this.setValue(result);

                });
        }
        else {
            this.ParameterId = 0;
        }
    }


    public Save(values: Object) {
        let param: ParamModel = new ParamModel();
        param.IsActive = values["isActive"];
        param.IsDynamic = false;
        param.Name = values["parametername"];
        param.Type = values["parametertype"];
        param.ParentParameterId = null;
        param.Id = this.ParameterId;
        if (this.ParameterId == 0) {
            param.CreatedBy = this.appState.get('loggedInUserId');
            param.CreatedOn = new Date();
        }
        else {
            param.ModifiedBy = this.appState.get('loggedInUserId');;
            param.ModifiedOn = new Date();
        }
        this.hieraManager.saveParameter(param)
            .subscribe(result => {
                this.ParameterId = result.Id;
                if (param.Type == 'Hash') {
                    this.SaveChildParameters(this.ParameterId);
                }
                this.SaveHieraLevels(this.ParameterId);
            });
    }

    private SaveHieraLevels(parameterId: Number) {
        let param: Array<Param2HieraLevelModel> = new Array<Param2HieraLevelModel>();
        const control = <FormArray>this.form.controls['hieraList'];
        control.getRawValue().forEach(item => {
            let model: Param2HieraLevelModel = new Param2HieraLevelModel();
            model.Id = item.hieraParamId;
            model.DefaultValue = item.defaultvalue;
            model.HieraId = item.hieralevel;
            model.IsMandatory = item.isMandatory;
            model.PermittedRoles = [];
            item.roles.forEach(role => {
                model.PermittedRoles.push(role.id)
            });
            model.ParameterId = parameterId;
            param.push(model);
        });
        this.hieraManager.saveParam2Hiera(param)
            .subscribe(result => {
                this.setValue(result);
                this._notification.success("Status", "Parameter Saved successfully");
            });
    }

    private setParameterValue(Parameter: ParamModel) {
        this.parametername.setValue(Parameter.Name);
        this.parametertype.setValue(Parameter.Type);
        this.IsActive.setValue(Parameter.IsActive);
        this.CreatedBy = Parameter.CreatedBy;
        this.CreatedOn = Parameter.CreatedOn;
        this.ModifiedBy = Parameter.ModifiedBy;
        this.ModifiedOn = Parameter.ModifiedOn;
    }
    private setValue(Param2Hieras: Array<Param2HieraLevelModel>) {
        const control = <FormArray>this.form.controls['hieraList'];
        // let i: number = 0;
        var len = control.length;
        for (var j = len - 1; j >= 0; j--) {
            this.removeHieralevel(j);
        }
        for (var i = 0; i < Param2Hieras.length; i++) {
            this.AddHiera();
            control.at(i).setValue({
                hieraParamId: Param2Hieras[i].Id,
                hieralevel: Param2Hieras[i].HieraId,
                defaultvalue: Param2Hieras[i].DefaultValue,
                isMandatory: Param2Hieras[i].IsMandatory,
                roles: this.allRoles.filter(function (e) {
                    return Param2Hieras[i].PermittedRoles.indexOf(e.id) > -1;
                })
            });
        }
    }

    private addChildParameter(i: number, ParamId: Number, ParamName: string) {
        const control = <FormArray>this.form.controls['ChildList'];
        if (ParamName == '')
            ParamName = 'Dynamic';
        if (ParamId === 0)
            ParamId = this.ParameterId;
        control.push(this.initChild(ParamId, ParamName));
    }

    private removeChildParameter(i: number) {
        const control = <FormArray>this.form.controls['ChildList'];
        var ParamId = control.getRawValue()[i].ParamId;
        control.removeAt(i);
        var len = control.length;
        for (var j = len - 1; j >= 0; j--) {
            if (control.getRawValue()[j].ParentParentId === ParamId)
                control.removeAt(j);
        }

    }

    private initChild(ParamId: Number, ParamName: string) {

        return this.fb.group({
            ParamId: [this.guid()],
            ParentParentId: [ParamId],
            ParentParamName: [ParamName],
            isDynamic: [false],
            ChildParamName: ['']
        });
    }

    private SaveChildParameters(parameterId: Number) {
        let param: Array<ParamModel> = new Array<ParamModel>();
        const control = <FormArray>this.form.controls['ChildList'];
        control.getRawValue().forEach(item => {
            let model: ParamModel = new ParamModel();
            model.IsActive = true;
            model.IsDynamic = item.isDynamic;
            model.Name = item.ChildParamName;
            model.Type = 'Hash';
            model.ParentParameterId = item.ParentParentId;
            model.Id = item.ParamId;
            if (item.ParamId == 0) {
                model.CreatedBy = this.appState.get('loggedInUserId');
                model.CreatedOn = new Date();
            }
            else {
                model.ModifiedBy = this.appState.get('loggedInUserId');;
                model.ModifiedOn = new Date();
            }

            param.push(model);
        });
        this.hieraManager.saveParameters(param)
            .subscribe(result => {
                this.setChildParamValue(result);
                this._notification.success("Status", "Child Parameters Saved successfully");
            });
    }


    private setChildParamValue(Params: Array<ParamModel>) {
        const control = <FormArray>this.form.controls['ChildList'];
        // let i: number = 0;
        var len = control.length;
        for (var j = len - 1; j >= 0; j--) {
            this.removeChildParameter(j);
        }
        for (var i = 0; i < Params.length; i++) {
            this.AddHiera();
            control.at(i).setValue({
                ParamId: Params[i].Id,
                ParentParentId: Params[i].ParentParameterId,
                isDynamic: Params[i].IsDynamic,
                ChildParamName: Params[i].Name,
                ParentParamName: Params[i].ParentParameterId
            });
        }
    }

    private guid() {
        return this.RenadomChars() + this.RenadomChars() + '-' + this.RenadomChars() + '-' + this.RenadomChars() + '-' +
            this.RenadomChars() + '-' + this.RenadomChars() + this.RenadomChars() + this.RenadomChars();
    }
    private RenadomChars() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    public Cancel() {
    }
    nodes = [

    ];

}
