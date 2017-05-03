export class ActionTaskModel {
	ActionId: string;
	Name: string;
	Description: string;
	Summary: string;
	CreatedBy: string;
	UpdatedBy: string;
	CreatedOn: Date;
	UpdatedOn: Date;
	module: string;
	menupath: string;
	Actiontype: string;
	Codelanguage: string;
	RemoteCode: string;
	AccessCode: string;
	Inputs: { [key: string]: any; }
	Outputs: { [key: string]: any; }
	Results: { [key: string]: any; }
	MockInputs: { [key: string]: any; }
	IsActive: boolean;
	TimeOut: Number;
}

