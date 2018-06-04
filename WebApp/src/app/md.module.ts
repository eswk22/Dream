/* This module is an example how you can package all services and components
 from Angular2-Material into one Angular2 module, which you can import in other modules
 */
import { NgModule, ModuleWithProviders } from '@angular/core';

import { MatDialogModule } from '@angular/material';
import { MatButtonModule } from '@angular/material';
import { MatButtonToggleModule } from '@angular/material';
import { MatCardModule } from '@angular/material';
import { MatCheckboxModule } from '@angular/material';
import { MatChipsModule } from '@angular/material';
import { MatDatepickerModule } from '@angular/material';
import { MatSelectModule } from '@angular/material';
import { MatGridListModule } from '@angular/material';
import { MatIconModule } from '@angular/material';
import { MatInputModule } from '@angular/material';
import { MatListModule } from '@angular/material';
import { MatMenuModule } from '@angular/material';
import { MatProgressBarModule } from '@angular/material';
import { MatProgressSpinnerModule } from '@angular/material';
import { MatRadioModule } from '@angular/material';
import { MatSidenavModule } from '@angular/material';
import { MatSliderModule } from '@angular/material';
import { MatSlideToggleModule } from '@angular/material';
import { MatTabsModule } from '@angular/material';
import { MatToolbarModule } from '@angular/material';
import { MatTooltipModule } from '@angular/material';



@NgModule({
	imports: [
		MatButtonModule,
		MatButtonToggleModule,
		MatCardModule,
		MatCheckboxModule,
		MatChipsModule, MatDatepickerModule, MatDialogModule, MatSelectModule,
		MatGridListModule,
		MatIconModule,
		MatInputModule,
		MatListModule,
		MatMenuModule,
		MatProgressBarModule,
		MatProgressSpinnerModule,
		MatRadioModule,
		MatSidenavModule,
		MatSliderModule,
		MatSlideToggleModule,
		MatTabsModule,
		MatToolbarModule,
        MatTooltipModule,
        MatDialogModule
	],
	exports: [
		MatButtonModule,
		MatButtonToggleModule,
		MatCardModule,
		MatCheckboxModule,
		MatChipsModule, MatDatepickerModule, MatDialogModule, MatSelectModule,
		MatGridListModule,
		MatIconModule,
		MatInputModule,
		MatListModule,
		MatMenuModule,
		MatProgressBarModule,
		MatProgressSpinnerModule,
		MatRadioModule,
		MatSidenavModule,
		MatSliderModule,
		MatSlideToggleModule,
		MatTabsModule,
		MatToolbarModule,
        MatTooltipModule,
        MatDialogModule
	]
})

export class MdModule {

	static forRoot(): ModuleWithProviders {
		return {
			ngModule: MdModule
		};
	}
}
