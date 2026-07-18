import { Component } from '@angular/core';

@Component({
  selector: 'app-forbidden',
  template: `
    <div class="forbidden">
      <h1>Access denied</h1>
      <p>You don't have permission to view this page. Check with your administrator if you believe this is a mistake.</p>
    </div>
  `,
  styles: `
    .forbidden {
      padding: 3rem 1.5rem;
      max-width: 520px;
      color: #455a64;
    }
    h1 { font-size: 1.5rem; margin: 0 0 0.5rem; }
  `,
})
export class Forbidden {}
