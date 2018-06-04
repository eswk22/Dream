import { Routes, RouterModule } from '@angular/router';

import { Edit } from './edit/edit.component';

import { HieraComponent } from './hieras.component';
import { ParamCreate } from './paramcreate/paramcreate.component';

// noinspection TypeScriptValidateTypes
const routes: Routes = [
    {
        path: 'hiera',
        component: HieraComponent,
        children: [
            { path: 'edit', component: Edit },
            { path: 'Parameteredit', component: ParamCreate },
            { path: 'Parameteredit/:ParameterId', component: ParamCreate }
        ]
    }
];

export const routing = RouterModule.forChild(routes);
