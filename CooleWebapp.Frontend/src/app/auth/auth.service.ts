import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, empty, Observable, of, throwError } from "rxjs";
import { map, flatMap, tap, take, filter } from "rxjs/operators";

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

  constructor(
    private readonly http: HttpClient
  ) {
    const serializedTokens = localStorage.getItem('tokens');
    if (!!serializedTokens) {
      this.tokensSub.next(JSON.parse(serializedTokens));
    }
    this.loggedIn = this.tokensSub.pipe(map(tokens => !!tokens));
  }

  private getTokens(body: URLSearchParams): Observable<AuthenticationResponse> {
    body.set('scope', 'offline_access profile openid');
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
  }

  private refresh(tokens: AuthenticationResponse): Observable<AuthenticationResponse> {
    const body = new URLSearchParams();
    body.set('grant_type', 'refresh_token');
    body.set('refresh_token', tokens.refresh_token);
    return this.getTokens(body);
  }

  public getAutoRefreshedToken(): Observable<AuthenticationResponse | null> {
    return this.tokensSub.pipe(
      filter(tokens => tokens !== 'refreshing'),
      flatMap(tokens => {
        if (!tokens) {
          return of(null);
        }
        if (tokens == 'refreshing') {
          return empty();
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
