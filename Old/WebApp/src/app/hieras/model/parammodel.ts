
export class ParamModel {
    constructor(

    ) { }

    public Id: Number;
    public Name: string;
    public Type: string;
    public ParentParameterId: Number;
    public IsDynamic: boolean;
    public IsActive: boolean;
    public ModifiedBy: string;
    public ModifiedOn: Date;
    public CreatedBy: string;
    public CreatedOn: Date;
}

export class Param2HieraLevelModel {
    constructor(

    ) { }

    public Id: Number;
    public ParameterId: Number;
    public HieraId: Number;
    public DefaultValue: string;
    public IsMandatory: boolean;
    public PermittedRoles: Array<string>;
}