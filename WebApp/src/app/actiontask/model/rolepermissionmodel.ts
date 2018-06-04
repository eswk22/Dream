


export class RolePermission {
	constructor(
	) {
		this.IsEdit = false;
		this.IsAdmin = false;
		this.IsExecute = false;
		this.IsView = false;
	}

	public ActionTaskId: string;
	public RoleId: string;
	public RoleName: string;
	public IsView: boolean;
	public IsEdit: boolean;
	public IsExecute: boolean;
	public IsAdmin: boolean;
	public IsActive: boolean;
}
