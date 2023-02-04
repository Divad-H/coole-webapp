import { ChangeDetectionStrategy, Component } from "@angular/core";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { AuthService } from "../../auth/auth.service";
import { UserBalance, UserBalanceData } from "../services/user-balance.service";
import { SidenavService } from "../sidenav/sidenav.service";

@Component({
  selector: 'cw-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ToolbarComponent {

  public readonly userBalance: Observable<UserBalanceData | null>;

  constructor(
    private readonly sidenavService: SidenavService,
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly userBalanceService: UserBalance,
  ) {

    this.userBalance = userBalanceService.userBalance;
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
    this.router.navigate(['/auth/login']);
  }
}
