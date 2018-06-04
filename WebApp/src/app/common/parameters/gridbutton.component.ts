import { Component } from "@angular/core";
import { ICellRendererAngularComp } from "ag-grid-angular/main";

@Component({
	selector: 'child-cell',
	template: `<span> <button md-button (click)="invokeParentMethod()"><i class="zmdi zmdi-edit"></i>Edit</button></span>`
})
export class GridButtonComponent implements ICellRendererAngularComp {
	public params: any;

	agInit(params: any): void {
		this.params = params;
	}

	public invokeParentMethod() {
		this.params.context.componentParent.OnGridButtonClicked(this.params.node.rowIndex,this.params.data);
	}
}