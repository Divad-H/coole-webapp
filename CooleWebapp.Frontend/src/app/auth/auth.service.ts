import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, EMPTY, Observable, of } from "rxjs";
import { map, tap, take, filter, shareReplay, catchError, mergeMap } from "rxjs/operators";
import jwt_decode from "jwt-decode";
import { Router } from "@angular/router";

export interface AuthenticationResponse {
  access_token: string,
  expires_in: number,
  expiresAt: Date,
  id_token: string,
  refresh_token: string,
  token_type: string,
}

@Injectable()
export class AuthService {

  private readonly tokensSub = new BehaviorSubject<AuthenticationResponse | 'refreshing' | null>(null);
  public readonly loggedIn: Observable<boolean>;
  public readonly roles: Observable<string[]>;

  constructor(
    private readonly http: HttpClient,
    private readonly router: Router,
  ) {
    const serializedTokens = localStorage.getItem('tokens');
    if (!!serializedTokens) {
      const deserialized = JSON.parse(serializedTokens);
      if (deserialized.expiresAt) {
        deserialized.expiresAt = new Date(deserialized.expiresAt);
      }
      this.tokensSub.next(deserialized);
    }
    this.loggedIn = this.tokensSub.pipe(map(tokens => !!tokens));
    this.roles = this.tokensSub.pipe(
      filter(tokens => tokens !== 'refreshing'),
      map(tokens => {
        if (!tokens) {
          return [];
        }
        if (tokens == 'refreshing') {
          return [];
        }
        try {
          var decoded = jwt_decode(tokens.id_token) as any;
          if (!decoded.role) {
            return []
          }
          if (Array.isArray(decoded.role)) {
            return decoded.role;
          }
          return [decoded.role];
        } catch (e) {
          console.error(e);
          return [];
        }
      }),
      shareReplay(1)
    )
  }

  private getTokens(body: URLSearchParams): Observable<AuthenticationResponse> {
    body.set('scope', 'offline_access profile openid roles');
    let options = {
      headers: new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    };
    return this.http.post('/connect/token', body.toString(), options).pipe(
      map(res => ({ ...res, expiresAt: new Date(Date.now() + (res as any).expires_in * 1000) } as AuthenticationResponse)),
      tap(res => {
        localStorage.setItem('tokens', JSON.stringify(res));
        this.tokensSub.next(res);
      })
    );
  }

  public login(username: string, password: string): Observable<AuthenticationResponse> {
    const body = new URLSearchParams();
    body.set('grant_type', 'password');
    body.set('username', username);
    body.set('password', password);
    return this.getTokens(body);
  }

  public logout() {
    this.tokensSub.next(null);
    localStorage.removeItem('tokens');
    this.router.navigate(['/auth/login']);
  }

  private refresh(tokens: AuthenticationResponse): Observable<AuthenticationResponse> {
    const body = new URLSearchParams();
    body.set('grant_type', 'refresh_token');
    body.set('refresh_token', tokens.refresh_token);
    return this.getTokens(body).pipe(
      catchError(e => {
        this.logout();
        return EMPTY;
      }),
    );
  }

  public getAutoRefreshedToken(): Observable<AuthenticationResponse | null> {
    return this.tokensSub.pipe(
      filter(tokens => tokens !== 'refreshing'),
      mergeMap(tokens => {
        if (!tokens) {
          return of(null);
        }
        if (tokens == 'refreshing') {
          return EMPTY;
        }
        // Refresh the token if it expires in less than one minute
        if (tokens.expiresAt < new Date(Date.now() + 60 * 1000)) {
          this.tokensSub.next('refreshing');
          return this.refresh(tokens);
        }
        return of(tokens);
      }),
      take(1)
    );
  }
}
