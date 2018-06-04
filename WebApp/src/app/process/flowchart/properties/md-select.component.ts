import { Component, ViewContainerRef, ViewChild } from "@angular/core";
import { ICellEditorAngularComp } from "ag-grid-angular/main";

@Component({
	selector: 'radio-cell',
	template: `
        <div class="container" #group tabindex="0" (keydown)="onKeyDown($event)">
            <mat-select [(ngModel)]="favouriteItem">
                <mat-option *ngFor="let item of items" [value]="item">
                    {{ item }}
                </mat-option>
            </mat-select>
        </div>
    `,
	styles: [
		`
            .container {
                border: 1px solid grey;
                background: #fff;
                width: 190px;
                height: 35px;
                padding-left: 15px;
            }
            
            .container:focus {
                outline: none;
            }
        `
	]
})
export class MdSelectComponent implements ICellEditorAngularComp {
	private params: any;

	private items: string[];
	private favouriteItem: string;
	private selectedIndex: number;

	@ViewChild('group', { read: ViewContainerRef }) public group;

	agInit(params: any): void {
		this.params = params;

		this.favouriteItem = this.params.value;
		this.items = this.params.items;

		this.selectedIndex = this.items.findIndex((item) => {
			return item === this.params.value;
		});
	}

	// dont use afterGuiAttached for post gui events - hook into ngAfterViewInit instead for this
	ngAfterViewInit() {
		this.group.element.nativeElement.focus();
		this.selectFavouriteItemBasedOnSelectedIndex();
	}

	private selectFavouriteItemBasedOnSelectedIndex() {
		this.favouriteItem = this.items[this.selectedIndex];
	}

	getValue() {
		return this.favouriteItem;
	}

	isPopup(): boolean {
		return true;
	}

    /*
     * A little over complicated for what it is, but the idea is to illustrate how you might navigate through the radio
     * buttons with up & down keys (instead of finishing editing)
     */
	onKeyDown(event): void {
		let key = event.which || event.keyCode;
		if (key === 38 || key === 40) {
			this.preventDefaultAndPropagation(event);

			if (key == 38) {            // up
				this.selectedIndex = this.selectedIndex === 0 ? (this.items.length - 1) : this.selectedIndex - 1;
			} else if (key == 40) {     // down
				this.selectedIndex = (this.selectedIndex === this.items.length - 1) ? 0 : this.selectedIndex + 1;
			}
			this.selectFavouriteItemBasedOnSelectedIndex();
		}
	}

	private preventDefaultAndPropagation(event) {
		event.preventDefault();
		event.stopPropagation();
	}
}