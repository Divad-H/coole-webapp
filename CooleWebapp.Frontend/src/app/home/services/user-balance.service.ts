import { Injectable } from "@angular/core";
import { catchError, map, Observable, of, shareReplay, switchMap } from "rxjs";
import { CooleWebappApi } from "../../../generated/coole-webapp-api";
import { AuthService } from "../../auth/auth.service";

export interface UserBalanceData {
  userName: string;
  balance: number;
}

@Injectable()
export class UserBalance {

  public readonly userBalance: Observable<UserBalanceData | null>;

  constructor(
    private readonly auth: AuthService,
    private readonly accountClient: CooleWebappApi.UserAccountClient,
  ) {
    this.userBalance = this.auth.roles.pipe(
      map(roles => roles.includes('User')),
      switchMap(isUser => isUser
        ? accountClient.getBalance().pipe(
          catchError(err => of(null))
        )
        : of(null)),
      shareReplay(1)
    )
  }
}
