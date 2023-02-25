import { Injectable } from '@angular/core';
import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest
} from '@angular/common/http';

import { mergeMap, Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private readonly authService: AuthService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (req.url === '/connect/token') {
      return next.handle(req);
    }
    return this.authService.getAutoRefreshedToken().pipe(
      mergeMap(r => {
        if (r) {
          req = req.clone({ setHeaders: { Authorization: `Bearer ${r.access_token}` } });
        }
        return next.handle(req);
      })
    );
  }
}
