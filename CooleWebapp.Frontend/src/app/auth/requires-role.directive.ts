import { Directive, Input, OnDestroy, OnInit, TemplateRef, ViewContainerRef } from "@angular/core";

import { BehaviorSubject, combineLatest, Subscription } from "rxjs";
import { distinctUntilChanged } from "rxjs/operators";

import { AuthService } from "./auth.service";

@Directive({
  selector: '[requiresRole]'
})
export class RequiresRoleDirective implements OnInit, OnDestroy {

  private readonly subscription = new Subscription();
  private readonly requiredRole = new BehaviorSubject<string[] | null>(null);

  @Input() set requiresRole(role: string | string[]) {
    if (typeof role === 'string') {
      this.requiredRole.next([role]);
    } else {
      this.requiredRole.next(role);
    }
  }

  constructor(
    private viewContainer: ViewContainerRef,
    private template: TemplateRef<any>,
    private authService: AuthService,
  ) {
  }

  ngOnInit(): void {
    this.subscription.add(
      combineLatest(
        this.authService.roles,
        this.requiredRole,
        (availRoles, roles) => roles?.some(role => availRoles.includes(role ?? ''))
      ).pipe(
        distinctUntilChanged()
      ).subscribe(visible => {
        if (visible) {
          this.viewContainer.createEmbeddedView(this.template);
        } else {
          this.viewContainer.clear();
        }
      })
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
