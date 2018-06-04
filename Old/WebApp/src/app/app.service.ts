import { Injectable } from '@angular/core';
import { Subject }    from 'rxjs/Subject';


export type InternalStateType = {
  [key: string]: any
};

@Injectable()
export class AppState {

    public get ServiceUrl() {
		return 'http://localhost:26402/';  
    }

  public _state: InternalStateType = { };

  // already return a clone of the current state
  public get state() {
    return this._state = this._clone(this._state);
  }
  // never allow mutation
  public set state(value) {
    throw new Error('do not mutate the `.state` directly');
  }

  public get(prop?: any) {
    // use our state getter for the clone
    const state = this.state;
    return state.hasOwnProperty(prop) ? state[prop] : state;
  }

  public set(prop: string, value: any) {
    // internally mutate our state
    return this._state[prop] = value;
  }

  private _clone(object: InternalStateType) {
    // simple object clone
    return JSON.parse(JSON.stringify( object ));
  }





  private _data = new Subject<Object>();
  private _dataStream$ = this._data.asObservable();

  private _subscriptions: Map<string, Array<Function>> = new Map<string, Array<Function>>();

  constructor() {
	  this._dataStream$.subscribe((data) => this._onEvent(data));
  }

  notifyDataChanged(event, value) {

	  let current = this._data[event];
	  if (current !== value) {
		  this._data[event] = value;

		  this._data.next({
			  event: event,
			  data: this._data[event]
		  });
	  }
  }

  subscribe(event: string, callback: Function) {
	  console.log('subscribe event');
	  let subscribers = this._subscriptions.get(event) || [];
	  subscribers.push(callback);

	  this._subscriptions.set(event, subscribers);
  }

  _onEvent(data: any) {
	  let subscribers = this._subscriptions.get(data['event']) || [];

	  subscribers.forEach((callback) => {
		  callback.call(null, data['data']);
	  });

  }


}
