import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { ASSIGNMENT_CONFIGS } from './assignment/assignment.model';
import { LOOKUP_ENTITIES } from './entities';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  protected readonly entities = LOOKUP_ENTITIES;
  protected readonly assignments = ASSIGNMENT_CONFIGS;
}
