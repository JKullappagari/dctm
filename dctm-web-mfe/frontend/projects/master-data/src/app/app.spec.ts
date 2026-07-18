import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { App } from './app';

describe('App', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [provideRouter([])],
    }).compileComponents();
  });

  it('creates the app', () => {
    const fixture = TestBed.createComponent(App);
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('exposes the lookup entities and assignment configs for the nav', () => {
    const app = TestBed.createComponent(App).componentInstance as unknown as {
      entities: unknown[];
      assignments: unknown[];
    };
    expect(app.entities.length).toBeGreaterThan(0);
    expect(app.assignments.length).toBeGreaterThan(0);
  });
});
