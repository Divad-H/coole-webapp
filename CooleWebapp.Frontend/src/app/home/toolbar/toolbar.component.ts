import { ChangeDetectionStrategy, Component } from "@angular/core";
import { Router } from "@angular/router";
import { AuthService } from "../../auth/auth.service";
import { SidenavService } from "../sidenav/sidenav.service";

@Component({
  selector: 'cw-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ToolbarComponent {

  constructor(
    private readonly sidenavService: SidenavService,
    private readonly authService: AuthService,
    private readonly router: Router,
  ) { }

  toggleSidenav() {
    this.sidenavService.toggle();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }
}
