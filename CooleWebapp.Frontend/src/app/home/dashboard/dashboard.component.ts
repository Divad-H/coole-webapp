import { ChangeDetectionStrategy, Component } from "@angular/core";
import { CooleWebappApi } from "../../../generated/coole-webapp-api";
import { Observable, shareReplay } from "rxjs";

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DashboardComponent {

  public forecasts: Observable<CooleWebappApi.IWeatherForecast[]>;

  constructor(
    private readonly weatherForecastClient: CooleWebappApi.WeatherForecastClient,
  ) {
    this.forecasts = weatherForecastClient.get().pipe(
      shareReplay(1)
    );
  }
}
