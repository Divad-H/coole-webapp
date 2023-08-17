import { ChangeDetectionStrategy, Component, Inject, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { BehaviorSubject, catchError, filter, of, switchMap, Subject, Subscriber, tap } from "rxjs";
import { CooleWebappApi } from "../../../../generated/coole-webapp-api";

@Component({
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserDetailsComponent implements OnInit, OnDestroy {

  public readonly form: FormGroup;
  public readonly userId?: number;

  public readonly submit = new Subject();
  private readonly errorResponseSubject = new BehaviorSubject('');
  readonly errorResponse = this.errorResponseSubject.asObservable();
  private readonly busySubject = new BehaviorSubject(false);
  readonly busy = this.busySubject.asObservable();
  private readonly subscriptions = new Subscriber();
    authService: any;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { user?: CooleWebappApi.IUserResponseModel },
    private readonly adminUsers: CooleWebappApi.AdminUsersClient,
    private readonly dialogRef: MatDialogRef<UserDetailsComponent>,
    readonly fb: FormBuilder
  ) {
    this.userId = data.user?.id;
    this.form = fb.group({
      isAdministrator: [data.user?.roles?.includes('Administrator'), []],
      isFridge: [data.user?.roles?.includes('Fridge'), []],
    });
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  ngOnInit(): void {

    this.subscriptions.add(
      this.submit.pipe(
        tap(() => this.errorResponseSubject.next('')),
        filter(() => this.form.valid),
        tap(() => this.busySubject.next(true)),
        switchMap(() => {
          const roles : CooleWebappApi.UserRole[] = ['Registered'];
          if (this.form.value.isAdministrator) {
            roles.push('Administrator');
          }
          if (this.form.value.isFridge) {
            roles.push('Fridge');
          } else {
            roles.push('User');
          }
          return this.adminUsers.editUser(
            new CooleWebappApi.EditUserRequestModel({
              userId: this.userId ?? 0,
              roles
            })
          ).pipe(
            catchError(e => {
              if (e?.error?.error_description) {
                this.errorResponseSubject.next(e.error.error_description);
              }
              return of({ error: true });
            })
          )
        })
      ).subscribe(res => {
        this.busySubject.next(false);
        if ((res as any)?.error) {
          return;
        }
        this.dialogRef.close(true);
      }));
  }

}
