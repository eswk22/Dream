import { Routes, RouterModule } from '@angular/router';

import { FlowChartComponent } from './flowchart/flowchart.component';
import { GeneralComponent } from './general/general.component';
import { ProcessListComponent } from './processlist/list.component';



// noinspection TypeScriptValidateTypes
const routes: Routes = [
    {
		path: 'flowchart',
		component: FlowChartComponent       
	},
	{
		path: 'process/general',
		component: GeneralComponent
	},
	{
		path: 'process/list',
		component: ProcessListComponent
	}
];

export const routing = RouterModule.forChild(routes);
