import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, from } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { MsalService } from '@azure/msal-angular';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private msalService: MsalService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return from(this.msalService.acquireTokenSilent({
      scopes: ['api://YOUR-API-IDENTIFIER/.default']
    }))
      .pipe(
        mergeMap((result) => {
          const accessToken = result.accessToken;
          if (accessToken) {
            request = request.clone({
              setHeaders: {
                Authorization: `Bearer ${accessToken}`
              }
            });
          }
          return next.handle(request);
        })
      );
  }
}
