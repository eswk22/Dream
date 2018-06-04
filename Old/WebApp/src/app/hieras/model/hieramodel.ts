
export class HieraModel {
    constructor(

    ) { }

    public Id: Number;
    public Name: string;
    public Version: string;
    public IsActive: boolean;
    public CreatedBy: string;
    public CreatedOn: Date;
    public ModifiedBy: string;
    public ModifiedOn: Date;
    public HieraLevels: Array<HieraLevelModel>;
}

export class HieraLevelModel {
    constructor(

    ) { }

    public Id: Number;
    public HieraId: Number;
    public Layer: string;
    public Level: Number;
    public IsActive: boolean;
}