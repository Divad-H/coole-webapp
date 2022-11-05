import { ChangeDetectionStrategy, Component } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, shareReplay } from "rxjs";
import { CooleWebappApi } from '../../generated/coole-webapp-api';
import { AuthService } from "../auth/auth.service";
import { SidenavService } from "./sidenav/sidenav.service";

@Component({
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HomeComponent {

  public forecasts: Observable<CooleWebappApi.IWeatherForecast[]>;
  public readonly isSidenavToggled: Observable<boolean>;

  constructor(
    private readonly weatherForecastClient: CooleWebappApi.WeatherForecastClient,
    private readonly authService: AuthService,
    private readonly router: Router,
    readonly sidenavService: SidenavService,
  ) {
    this.forecasts = weatherForecastClient.get().pipe(
      shareReplay(1)
    );

    this.isSidenavToggled = sidenavService.isToggled;
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }
}
