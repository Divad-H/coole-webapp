import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from "@angular/router";
import { map, take, Observable } from "rxjs";

import { AuthService } from "./auth.service";

@Injectable()
export class NotLoggedInGuard  {
  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    return this.authService.loggedIn.pipe(
      map((loggedIn) =>
        !loggedIn ? true : this.router.createUrlTree(['home'])
      ),
      take(1)
    );
  }

  canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    return this.canActivate(childRoute, state);
  }

  canLoad(
    route: Route,
    segments: UrlSegment[]
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.authService.loggedIn.pipe(
      map((loggedIn) =>
        !loggedIn ? true : this.router.createUrlTree(['auth/login'])
      ),
      take(1)
    );
  }
}
