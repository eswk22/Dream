export class CompilerResultModel {
	IsSuccess: boolean;
	Decompiled: string;
	Errors: Array<errorModel>;
	Warnings: Array<errorModel>;
	Infos: Array<errorModel>;
}

export class errorModel {
	Id: string;
	Message: string;
	Severity: string;
	//Start: linemodel
	//End: linemodel
}

export class linemodel {
	Column: Number;
	Line: Number;
}
