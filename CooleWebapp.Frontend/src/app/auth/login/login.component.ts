import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { BehaviorSubject, filter, map, tap, Subject, Subscriber, switchMap, catchError, of, take, withLatestFrom } from "rxjs";
import { CooleWebappApi } from "../../../generated/coole-webapp-api";

import { AuthService } from "../auth.service";

@Component({
  selector: 'cw-login',
  templateUrl: './login.component.html',
  styleUrls: ['../auth-styles.css'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class LoginComponent implements OnInit, OnDestroy {

  readonly form: FormGroup;
  readonly submit = new Subject();
  private readonly errorResponseSubject = new BehaviorSubject('');
  readonly errorResponse = this.errorResponseSubject.asObservable();
  private readonly busySubject = new BehaviorSubject(false);
  readonly busy = this.busySubject.asObservable();
  private readonly subscriptions = new Subscriber();
  private readonly emailUnconfirmedSubject = new BehaviorSubject<string | null>(null);
  readonly emailUnconfirmed = this.emailUnconfirmedSubject.asObservable();
  readonly resendVerificationEmailSubject = new Subject();

  constructor(
    formBuilder: FormBuilder,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly authService: AuthService,
    private readonly registrationClient: CooleWebappApi.RegistrationClient
  ) {
    this.form = formBuilder.group({
      email: [null, [Validators.required]],
      password: [null, [Validators.required]],
    })
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  ngOnInit(): void {
    this.subscriptions.add(
      this.route.queryParams.pipe(
        map(params => params['email']),
        filter(email => !!email)
      ).subscribe((email: string) => {
        const control = this.form.get('email');
        control?.patchValue(email);
        control?.updateValueAndValidity();
      })
    );

    this.subscriptions.add(
      this.submit.pipe(
        tap(() => this.errorResponseSubject.next('')),
        tap(() => this.emailUnconfirmedSubject.next(null)),
        filter(() => this.form.valid),
        tap(() => this.busySubject.next(true)),
        switchMap(() => this.authService.login(this.form.value.email, this.form.value.password).pipe(
          catchError(e => {
            if (e.error.error_description) {
              this.errorResponseSubject.next(e.error.error_description);
            }
            if (e.error.error === 'interaction_required') {
              this.emailUnconfirmedSubject.next(this.form.value.email);
            }
            return of({ error: true });
          })
        ))
      ).subscribe(res => {
        this.busySubject.next(false);
        if ((res as any).error) {
          return;
        }
        this.router.navigate(['/home']);
      }));

    this.subscriptions.add(
      this.resendVerificationEmailSubject.pipe(
        withLatestFrom(this.emailUnconfirmedSubject),
        tap(() => this.errorResponseSubject.next('')),
        tap(() => this.emailUnconfirmedSubject.next(null)),
        switchMap(([_, email]) => this.registrationClient.resendConfirmationEmail(
          new CooleWebappApi.ResendConfirmationEmailData({ email: email as string })).pipe(
          catchError(e => {
            if (e.error.error_description) {
              this.errorResponseSubject.next(e.error.error_description);
            }
            return of({ error: true });
          })
        ))
      ).subscribe()
    );
  }

  resendVerificationEmail(): void {
    this.resendVerificationEmailSubject.next({});
  }
}
