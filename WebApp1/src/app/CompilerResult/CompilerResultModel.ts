export class CompilerResultModel {
	isSuccess: boolean;
	decompiledResults: string;
	errors: Array<errorModel>;
	warnings: Array<warningModel>;
}

export class errorModel {
	id: string;
	message: string;
	severity: string;
}

export class warningModel {
	id: string;
	message: string;
	severity: string;
}
