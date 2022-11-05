import { ChangeDetectionStrategy, Component } from "@angular/core";
import { SidenavService } from "../sidenav/sidenav.service";

@Component({
  selector: 'cw-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ToolbarComponent {

  constructor(
    private readonly sidenavService: SidenavService
  ) { }

  toggleSidenav() {
    this.sidenavService.toggle();
  }
}
