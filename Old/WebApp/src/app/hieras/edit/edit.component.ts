import {
    Component,
    OnInit
} from '@angular/core';

import { AppState } from '../../app.service';
import { HieraService } from '../hieras.service';
import { Router } from '@angular/router';
import { FormBuilder, Validators, FormGroup, FormArray, FormControl } from '@angular/forms';

import { HieraModel, HieraLevelModel } from '../model/hieramodel';

@Component({
    // The selector is what angular internally uses
    // for `document.querySelectorAll(selector)` in our index.html
    // where, in this case, selector is the string 'home'
    selector: 'hiera',  // <home></home>
    // We need to tell Angular's Dependency Injection which providers are in our app.
    providers: [
        HieraService
    ],
    // Our list of styles in our component. We may add more to compose many styles together
    //styleUrls: ['./hieras.component.css'],
    // Every Angular template is first compiled by the browser before Angular runs it's compiler
    templateUrl: './edit.component.html'
})
export class Edit implements OnInit {
    // Set our default values
    public localState = { value: '' };
    hieraForm: FormGroup;
    _hieramodel: HieraModel;
    versionsArray: string[] = [];
    hieraname: string = "Default";

    // TypeScript public modifiers
    constructor(
        public fb: FormBuilder,
        public router: Router,
        public appState: AppState,
        public hieraManager: HieraService
    ) {
        this.hieraForm = this.fb.group({
            Name: ["", Validators.required],
            versions: ["", Validators.required],
            CreatedBy: ["", Validators.required, { disabled: true }],
            CreatedOn: ["", Validators.required,{ disabled: true }],
            ModifiedBy: ["", Validators.required],
            ModifiedOn: ["", Validators.required],
            hieraList: this.fb.array([
                this.initHiera(),
            ])
        });
    }


    private initHiera() {
        return this.fb.group({
            Layer: ['', Validators.required]
        });
    }

    private onVersionChange(newValue) {
        console.log(newValue);
        this.LoadHieraData(this.hieraname, newValue);
    }

    private addHieralevel(i: number) {
        const control = <FormArray>this.hieraForm.controls['hieraList'];
        control.insert(i, this.initHiera());
    }
    private removeHieralevel(i: number) {
        // remove address from the list
        const control = <FormArray>this.hieraForm.controls['hieraList'];
        control.removeAt(i);
    }

    private LoadVersions() {
        let name: string = this.hieraname;
        this.hieraManager.getHieraVersions(name)
            .subscribe(result => {
                this.versionsArray = result;
            });
    }

    private LoadHieraData(name: string, version: number) {

        this.hieraManager.getHiera(name, version)
            .subscribe(result => {
                this.setValue(result);
            });
    }

    private setValue(hieramodel: HieraModel) {
        const control = <FormArray>this.hieraForm.controls['hieraList'];
        let i: number = 1;
        for (var j = 1; j < control.length; j++) {
            this.removeHieralevel(j);
        }
        hieramodel.HieraLevels.forEach(item => {
            if (control.length < i) {
                this.addHieralevel(i);
            };
            control.at(i-1).setValue({ Layer: item.Layer });
            i++;
        });
        console.log(hieramodel.HieraLevels);
    }

    private Save() {
        const control = <FormArray>this.hieraForm.controls['hieraList'];
        let hiera: HieraModel = new HieraModel();
        hiera.Name = this.hieraname;
        hiera.IsActive = true;
        let hieraLevels: Array<HieraLevelModel> = new Array<HieraLevelModel>();
        let i: number = 1;
        control.getRawValue().forEach(item => {
            let hieralevel: HieraLevelModel = new HieraLevelModel();
            hieralevel.Id = 0;
            hieralevel.IsActive = true;
            hieralevel.HieraId = 0;
            hieralevel.Layer = item.Layer;
            hieralevel.Level = i++;
            hieraLevels.push(hieralevel);
        });
        hiera.HieraLevels = hieraLevels;

        this.hieraManager.saveHiera(hiera)
            .subscribe(result => {
                this.ngOnInit();
            });

    }

    private Cancel() {
        this.ngOnInit();
    }

 

    public ngOnInit() {
        let name: string = this.hieraname;
        this.hieraManager.getHieraVersions(name)
            .subscribe(result => {
                this.versionsArray = result;
                let version: number = Math.max.apply(null, this.versionsArray);
                console.log(this.versionsArray);
                this.hieraForm.controls["versions"].setValue(version);
    //            this.LoadHieraData(name, version);
            });

    }



}
