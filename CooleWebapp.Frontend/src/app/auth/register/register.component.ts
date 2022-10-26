import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, ValidationErrors, Validators } from "@angular/forms";
import { ErrorStateMatcher } from "@angular/material/core";
import { BehaviorSubject, catchError, filter, map, mapTo, of, Subject, Subscriber, switchMap, tap } from "rxjs";
import { CustomValidators } from "../../utilities/cutom-validators";
import { ParentErrorStateMatcher } from "../../utilities/error-state-matchers";
import { CooleWebappApi } from '../../../generated/coole-webapp-api';

@Component({
  selector: 'cw-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class RegisterComponent implements OnInit, OnDestroy {

  readonly form: FormGroup;
  readonly parentErrorStateMatcher: ErrorStateMatcher;
  readonly submit = new Subject();
  private readonly errorResponseSubject = new BehaviorSubject('');
  readonly errorResponse = this.errorResponseSubject.asObservable();
  private readonly busySubject = new BehaviorSubject(false);
  readonly busy = this.busySubject.asObservable();
  private readonly subscriptions = new Subscriber();

  constructor(
    formBuilder: FormBuilder,
    private readonly registration: CooleWebappApi.RegistrationClient,
  ) {
    this.parentErrorStateMatcher = new ParentErrorStateMatcher();
    this.form = formBuilder.group({
      title: [null],
      name: [null, Validators.required],
      initials: [
        null,
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(3),
          CustomValidators.containsNoWhitespace
        ]
      ],
      email: [null, [Validators.required, Validators.email]],
      passwords: formBuilder.group(
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
        { validator: this.passwordsMatch }
      ),
    })
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  ngOnInit(): void {
    const initialsControl = this.form.get('initials')!;
    this.subscriptions.add(
      this.form.get('name')!.valueChanges.pipe(
        map((v: string) => {
          if (!v?.length) {
            return '';
          }
          const words = v.split(' ');
          if (!words.length) {
            return '';
          }
          return `${words[0][0] ?? ''}${words[words.length - 1][0] ?? ''}${words[words.length - 1][words[words.length - 1].length - 1] ?? ''}`
            .toUpperCase();
        }),
      ).subscribe(initials => {
        if (initialsControl.dirty) {
          if (initialsControl.value) {
            return;
          }
          initialsControl.markAsPristine();
        }
        initialsControl.patchValue(initials);
        initialsControl.updateValueAndValidity();
      })
    );

    this.subscriptions.add(
      this.submit.pipe(
        tap(() => this.errorResponseSubject.next('')),
        filter(() => this.form.valid),
        tap(() => this.busySubject.next(true)),
        switchMap(() => this.registration.registerUser(new CooleWebappApi.RegistrationData({
          title: this.form.value.name,
          email: this.form.value.email,
          name: this.form.value.name,
          initials: this.form.value.initials,
          password: this.form.value.passwords.password,
        })).pipe(
          mapTo({}),
          catchError(e => {
            if (e.message) {
              this.errorResponseSubject.next(e.message);
            }
            return of({ error: true });
          }),
        )),
      ).subscribe(res => {
        this.busySubject.next(false);
        if ((res as any).error) {
          return;
        }
        // router.navigate(./check-your-email)
      })
    );
  }

  passwordsMatch(group: FormGroup): ValidationErrors | null {
    const password = group.get('password')!.value;
    const confirmedPassword = group.get('confirmPassword')!.value;

    return password === confirmedPassword
      ? null
      : { passwordsMatch: true };
  }
}
