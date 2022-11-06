import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, CanLoad, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from "@angular/router";
import { map, take, Observable } from "rxjs";
import { AuthService } from "./auth.service";

@Injectable()
export class RoleGuard implements CanActivate, CanLoad {

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) { }

  private permitted(roles: string[]): Observable<boolean | UrlTree> {
    return this.authService.roles.pipe(
      map(availRoles =>
        roles.some(role => availRoles.includes(role))
        || this.router.createUrlTree(['home'])
      ),
      take(1)
    );
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    return this.permitted(route.data['roles'] ?? []);
  }

  canLoad(
    route: Route,
    segments: UrlSegment[]
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.permitted(route.data?.['roles'] ?? []);
  }
}
