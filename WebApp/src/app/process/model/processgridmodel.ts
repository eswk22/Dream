
export class ProcesslistModel {
	public Id: string;
	public Name: string;
	public Namespace: string;
	public ModifiedBy: string;
	public ModifiedOn: Date;
	public CreatedBy: string;
	public CreatedOn: string;
	public IsActive: boolean;
	public Summary: string;
}

export class ProcessGridModel {
	public data: Array<ProcesslistModel>;
	public totalElements: number;
}

