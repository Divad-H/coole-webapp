import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { BehaviorSubject, filter, map, tap, Subject, Subscriber, switchMap, catchError, of, take, mapTo } from "rxjs";
import { CooleWebappApi } from "../../../generated/coole-webapp-api";

import { AuthService } from "../auth.service";

@Component({
  selector: 'cw-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['../auth-styles.css'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class ResetPasswordComponent implements OnInit, OnDestroy {

  readonly form: FormGroup;
  readonly submit = new Subject();
  private readonly errorResponseSubject = new BehaviorSubject('');
  readonly errorResponse = this.errorResponseSubject.asObservable();
  private readonly busySubject = new BehaviorSubject(false);
  readonly busy = this.busySubject.asObservable();
  private readonly subscriptions = new Subscriber();

  constructor(
    formBuilder: FormBuilder,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly authService: AuthService,
    private readonly registration: CooleWebappApi.RegistrationClient,
  ) {
    this.form = formBuilder.group({
      email: [null, [Validators.required]],
    })
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  ngOnInit(): void {
    this.submit.pipe(
      tap(() => this.errorResponseSubject.next('')),
      filter(() => this.form.valid),
      tap(() => this.busySubject.next(true)),
      switchMap(() => this.registration.initiatePasswordReset(
        new CooleWebappApi.InitiatePasswordResetData({ email: this.form.value.email })).pipe(
          mapTo({}),
          catchError(e => {
            if (e.message) {
              this.errorResponseSubject.next(e.message);
            }
            return of({ error: true });
          })
        ))
    ).subscribe(res => {
      this.busySubject.next(false);
      if ((res as any).error) {
        return;
      }
      this.router.navigate(['../confirm-initiate-reset-password'], { relativeTo: this.route });
    })
  }
}
