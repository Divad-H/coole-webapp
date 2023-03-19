import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { BehaviorSubject, catchError, concat, map, mapTo, Observable, of, ReplaySubject, shareReplay, startWith, Subject, Subscription, switchMap, take, tap } from "rxjs";
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
  private readonly fridgeBalance = new BehaviorSubject<UserBalanceData | null>(null);
  private readonly fridgeUserId = new BehaviorSubject<number | null>(null);

  constructor(
    private readonly auth: AuthService,
    private readonly accountClient: CooleWebappApi.UserAccountClient,
    private readonly snackBar: MatSnackBar
  ) {
    this.userBalance = this.refreshSubject.pipe(
      switchMap(() => this.auth.roles.pipe(
        map(roles => roles.includes('User')),
        switchMap(isUser => 
          isUser
            ? concat(
              accountClient.getBalance().pipe(
                catchError(err => of(null))
              ) as Observable<UserBalanceData | null>,
              this.changedBalance)
            : concat(
              this.fridgeBalance,
              this.changedBalance)
          )
      )),
      shareReplay(1)
    )
  }

  public addBalance(amount: number): Observable<boolean> {
    return this.fridgeUserId.pipe(
      take(1),
      switchMap(id => (id == null
        ? this.accountClient.addBalance(new CooleWebappApi.AddBalanceRequestModel({ amount }))
        : this.accountClient.addBalance(new CooleWebappApi.AddBalanceRequestModel({ amount })) // TODO: fridge end point
      ).pipe(
          tap(res => this.changedBalance.next(res)),
          map(() => true),
          catchError(err => {
            this.snackBar.open('Could not add balance.', 'Close', { duration: 5000 });
            return of(false);
          }),
        ))
    );
  }

  public setFridgeBalance(balance: UserBalanceData, userId: number): Subscription {
    this.fridgeBalance.next(balance);
    this.fridgeUserId.next(userId);
    return new Subscription(() => {
      this.fridgeBalance.next(null);
      this.fridgeUserId.next(null);
    });
  }

  public refresh(): void {
    this.refreshSubject.next({});
  }
}
