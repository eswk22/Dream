import { Routes, RouterModule } from '@angular/router';

import { EditorComponent } from './editor.component';

// noinspection TypeScriptValidateTypes
const routes: Routes = [
    {
        path: 'editor',
        component: EditorComponent       
    }
];

export const routing = RouterModule.forChild(routes);
