import { ParameterModel } from './../../common/model/parametermodel';
import { RolePermission } from './rolepermissionmodel';

export class ActionTaskModel {
	constructor(
	) { }

	public ActionTaskId: string;
	public Name: string;
	public Namespace: string;
	public FolderPath: string;
	public Timeout: number;
	public Type: string;
	public Queuename: string;
	public IsActive: boolean;
	public Summary: string;
	public CreatedBy: string;
	public CreatedOn: Date;
	public ModifiedBy: string;
	public ModifiedOn: Date;
	public Description: string;
	public Version: string;
	public RemoteCode: string;
	public RemoteLanguage: string;
	public LocalCode: string;
	public LocalLanguage: string;
	public PermittedRoles: Array<RolePermission>;
	public Parameters: Array<ParameterModel>;
	public Status: string;
}



export class CodeModel {
	constructor(
	) { }
	public ActionTaskId: string;
	public language: string;
	public code: string;
}

export class ActionTasklistModel{
	constructor(
	) { }
	public ActionTaskId: string;
	public Name: string;
	public Namespace: string;
	public Assignedto: string;
}

export class LockActionTaskModel {
	public ActionTaskId: string;
	public LockByName: string;
	public CreatedBy: string;
	public CreatedOn: Date;
}

export class AtionTaskListModel {
	public ActionTaskId: string;
	public Name: string;
	public Namespace: string;
	public ModifiedBy: string;
	public ModifiedOn: Date;
	public CreatedBy: string;
	public CreatedOn: string;
	public IsActive: boolean;
	public Summary: string;
}

export class ActionTaskGridModel {
	public data: Array<ActionTasklistModel>;
	public totalElements: number;
}