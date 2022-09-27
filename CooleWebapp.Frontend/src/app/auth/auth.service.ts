import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { tap } from "rxjs/operators";

@Injectable()
export class AuthService {

  public tokens: any;

  constructor(
    private readonly http: HttpClient
  ) { }

  public login(username: string, password: string): Observable<any> {
    const body = new URLSearchParams();
    body.set('grant_type', 'password');
    body.set('username', username);
    body.set('password', password);
    body.set('scope', 'offline_access profile openid');
    let options = {
      headers: new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    };
    return this.http.post('/connect/token', body.toString(), options).pipe(
      tap(res => this.tokens = res)
    );
  }
}
