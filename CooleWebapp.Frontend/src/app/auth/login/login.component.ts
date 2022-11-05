import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { BehaviorSubject, filter, map, tap, Subject, Subscriber, switchMap, catchError, of, take } from "rxjs";

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

  constructor(
    formBuilder: FormBuilder,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly authService: AuthService,
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

    this.submit.pipe(
      tap(() => this.errorResponseSubject.next('')),
      filter(() => this.form.valid),
      tap(() => this.busySubject.next(true)),
      switchMap(() => this.authService.login(this.form.value.email, this.form.value.password).pipe(
        catchError(e => {
          if (e.error.error_description) {
            this.errorResponseSubject.next(e.error.error_description);
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
    })
  }
}
