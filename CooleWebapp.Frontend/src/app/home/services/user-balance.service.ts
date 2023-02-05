import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { BehaviorSubject, catchError, concat, map, mapTo, Observable, of, shareReplay, startWith, Subject, switchMap, tap } from "rxjs";
import { CooleWebappApi } from "../../../generated/coole-webapp-api";
import { AuthService } from "../../auth/auth.service";

export interface UserBalanceData {
  userName: string;
  balance: number;
}

@Injectable()
export class UserBalance {

  private readonly changedBalance = new Subject<UserBalanceData>();
  private readonly refreshSubject = new BehaviorSubject({});
  public readonly userBalance: Observable<UserBalanceData | null>;

  constructor(
    private readonly auth: AuthService,
    private readonly accountClient: CooleWebappApi.UserAccountClient,
    private readonly snackBar: MatSnackBar
  ) {
    this.userBalance = this.refreshSubject.pipe(
      switchMap(() => this.auth.roles.pipe(
        map(roles => roles.includes('User')),
        switchMap(isUser => concat(
          isUser
            ? accountClient.getBalance().pipe(
              catchError(err => of(null))
            )
            : of(null),
          this.changedBalance)),
      )),
      shareReplay(1)
    )
  }

  public addBalance(amount: number) : Observable<boolean> {
    return this.accountClient.addBalance(
      new CooleWebappApi.AddBalanceRequestModel({ amount })).pipe(
        tap(res => this.changedBalance.next(res)),
        mapTo(true),
        catchError(err => {
          this.snackBar.open('Could not add balance.', 'Close', { duration: 5000 });
          return of(false);
        }),
    );
  }

  public refresh(): void {
    this.refreshSubject.next({});
  }
}
