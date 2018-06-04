import { BSModalContext } from 'angular2-modal/plugins/bootstrap';

export class ParameterModelContext extends BSModalContext {
	public name: string;
	public type: string;
	public defaultvalue: string;
}