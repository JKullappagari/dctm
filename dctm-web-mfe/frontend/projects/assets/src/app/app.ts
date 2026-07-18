import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  template: `
    <header class="mfe-banner"><strong>Assets</strong><span>micro-frontend — standalone dev mode</span></header>
    <router-outlet />
  `,
  styles: `
    :host { display: flex; flex-direction: column; height: 100dvh; }
    .mfe-banner { display: flex; align-items: center; gap: 0.75rem; padding: 0.6rem 1.25rem; background: #263238; color: #eceff1; }
    .mfe-banner span { font-size: 0.8rem; opacity: 0.7; }
  `,
})
export class App {}
