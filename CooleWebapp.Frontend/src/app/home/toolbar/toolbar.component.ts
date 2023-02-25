import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { switchMap, Observable, Subject, Subscription } from "rxjs";
import { AuthService } from "../../auth/auth.service";
import { PayDialogComponent } from "../pay-dialog/pay-dialog.component";
import { UserBalance, UserBalanceData } from "../services/user-balance.service";
import { SidenavService } from "../sidenav/sidenav.service";

@Component({
  selector: 'cw-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ToolbarComponent implements OnInit, OnDestroy {

  private readonly subscriptions = new Subscription();
  public readonly userBalance: Observable<UserBalanceData | null>;
  public readonly topUpAccountSubject = new Subject();

  constructor(
    private readonly sidenavService: SidenavService,
    private readonly authService: AuthService,
    private readonly userBalanceService: UserBalance,
    private readonly dialog: MatDialog,
  ) {

    this.userBalance = userBalanceService.userBalance;
  }


  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  ngOnInit(): void {
    this.subscriptions.add(
      this.topUpAccountSubject.pipe(
        switchMap((product: any) => {
          const dialogRef = this.dialog.open(PayDialogComponent, {
            data: { product },
          });

          return dialogRef.afterClosed();
        })
      ).subscribe());
  }

  formatBalance(balance: number | undefined): string | null {
    const res = balance?.toFixed(2);
    if (res != null)
      return `${res} €`;
    return null;
  }

  toggleSidenav() {
    this.sidenavService.toggle();
  }

  logout() {
    this.authService.logout();
  }
}
