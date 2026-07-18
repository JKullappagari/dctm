import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { authInterceptor, provideAuth } from 'angular-auth-oidc-client';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes, withComponentInputBinding()),
    // The remote MFEs share this root injector when loaded through federation,
    // so the interceptor attaches tokens to their API calls too.
    provideHttpClient(withFetch(), withInterceptors([authInterceptor()])),
    provideAuth({
      config: {
        authority: 'http://localhost:8180/realms/dctrack',
        redirectUrl: window.location.origin,
        postLogoutRedirectUri: window.location.origin,
        clientId: 'dctm-shell',
        scope: 'openid profile email',
        responseType: 'code',
        silentRenew: true,
        useRefreshToken: true,
        // APIs that receive the Bearer token via authInterceptor: identity (8082),
        // master-data (8081), asset (8083), reporting (8084). Missing an entry means
        // that API gets no JWT → 401 under enforcement → blank pages.
        secureRoutes: [
          'http://localhost:8081',
          'http://localhost:8082',
          'http://localhost:8083',
          'http://localhost:8084',
        ],
      },
    }),
  ],
};
