import { Routes, RouterModule } from '@angular/router';

import { GeneralComponent } from './general/general.component';

import { ParameterComponent } from './../common/parameters/parameters.component';
import { ActionTaskListComponent } from './list/actiontask-list.component';


import { ATReferenceComponent } from './references/references.component';

// noinspection TypeScriptValidateTypes
const routes: Routes = [
    {
		path: 'general',
		component: GeneralComponent       
	},
	{
		path: 'parameters',
		component: ParameterComponent
	},
	{
		path: 'list',
		component: ActionTaskListComponent
	},
	{
		path: 'ATReferences',
		component: ATReferenceComponent
	}
];

export const routing = RouterModule.forChild(routes);
