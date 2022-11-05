import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { BehaviorSubject, filter, map, tap, Subject, Subscriber, switchMap, catchError, of, take, mapTo, withLatestFrom, throwError } from "rxjs";
import { CooleWebappApi } from "../../../generated/coole-webapp-api";
import { CustomValidators } from "../../utilities/cutom-validators";
import { ParentErrorStateMatcher } from "../../utilities/error-state-matchers";

import { AuthService } from "../auth.service";

@Component({
  selector: 'cw-finish-reset-password',
  templateUrl: './finish-reset-password.component.html',
  styleUrls: ['../auth-styles.css'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class FinishResetPasswordComponent implements OnInit, OnDestroy {

  readonly form: FormGroup;
  readonly submit = new Subject();
  private readonly errorResponseSubject = new BehaviorSubject('');
  readonly errorResponse = this.errorResponseSubject.asObservable();
  private readonly busySubject = new BehaviorSubject(false);
  readonly busy = this.busySubject.asObservable();
  private readonly subscriptions = new Subscriber();
  readonly parentErrorStateMatcher = new ParentErrorStateMatcher();

  constructor(
    formBuilder: FormBuilder,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly authService: AuthService,
    private readonly registration: CooleWebappApi.RegistrationClient,
  ) {
    this.form = formBuilder.group(
      {
        password: [
          '',
          [
            CustomValidators.containsNumber,
            CustomValidators.containsSpecialCharacter,
            CustomValidators.containsLowercase,
            CustomValidators.containsUppercase,
            Validators.minLength(8)
          ],
        ],
        confirmPassword: ['', Validators.required],
      },
      { validator: CustomValidators.passwordsMatch }
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  ngOnInit(): void {
    this.submit.pipe(
      tap(() => this.errorResponseSubject.next('')),
      filter(() => this.form.valid),
      tap(() => this.busySubject.next(true)),
      withLatestFrom(this.route.queryParams),
      switchMap(([_, params]) => (!params['email'] || !params['token']
        ? throwError(new Error('Invalid link, please check that the link is correct.'))
        : this.registration.resetPassword(
          new CooleWebappApi.ResetPasswordData({
            password: this.form.value.password,
            email: params['email'],
            token: params['token'],
          }))).pipe(
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
      this.router.navigate(['/auth/login']);
    })
  }
}
