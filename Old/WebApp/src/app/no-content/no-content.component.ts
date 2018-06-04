import { Component } from '@angular/core';

@Component({
  selector: 'no-content',
  template: `
    <div>
      <h1>404: page missing</h1>
    </div>
  `,
  styles: [require('./no-content.scss')],
})
export class NoContentComponent {
    constructor() {
        console.log('no conent');
    }
    
}
