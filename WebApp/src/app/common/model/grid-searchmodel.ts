
export class GridSearchModel {
	constructor(
	) {
		this.filterPerColumn = []; 
	}

	public page: number;
	public size: number;
	public sort: string;
	public quickFilter: string;
	public filterPerColumn: Array<FilterModel>;
}

export class FilterModel {
	constructor(
	) { }

	public column: string;
	public filter: string;
	public type: string;
}