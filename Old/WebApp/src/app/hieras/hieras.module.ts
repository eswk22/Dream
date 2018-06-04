import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgaModule } from '../theme/nga.module';

import { Edit } from './edit/edit.component';
import { ParamCreate } from './paramcreate/paramcreate.component';
import { routing } from './hieras.routes';
import { SelectModule } from 'ng2-select';
import { TreeModule } from 'angular2-tree-component';

console.log('`hiera` bundle loaded asynchronously');

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        NgaModule,
        routing,
        SelectModule,
        TreeModule
    ],
    declarations: [
        Edit,
        ParamCreate
    ]
})
export class HieraModule { }
