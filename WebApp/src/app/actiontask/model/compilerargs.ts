
export class CompilerArgs {
	constructor(
	) { }

	public Code: string;
	public SourceLangage: string;
	public TargetLanguage: string;
	public OptimizationsEnabled: boolean;
}

export class CompilationResult {
	constructor(
	) { }

	public Decompiled: string;
	public IsSuccess: boolean;
	public Errors: Array<CompilationResultDiagnostic>;
	public Warnings: Array<CompilationResultDiagnostic>;
	public Infos: Array<CompilationResultDiagnostic>;
}

export class CompilationResultDiagnostic {
	constructor(
	) { }

	public Id: string;
	public Severity: string;
	public Message: string;
	public Start: CompilationResultDiagnosticLocation;
	public End: CompilationResultDiagnosticLocation;
}
export class CompilationResultDiagnosticLocation {
	constructor(
	) { }
	public Line: number;
	public Column: number;
}