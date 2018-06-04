import { inputParams,outputParams } from './parametermodel';

export class PropertyModel {
	constructor(
	) { }
	public name: string;
	public objectid: string;
	public objectType: string;
	public mergeType: string;
	public severityType: string;
	public condition: string;
	public input: Array<inputParams>;
	public output: Array<outputParams>;

}

