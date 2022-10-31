import { ChangeDetectionStrategy, Component } from "@angular/core";
import { Observable, shareReplay } from "rxjs";
import { CooleWebappApi } from '../../generated/coole-webapp-api';

@Component({
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HomeComponent {

  public forecasts: Observable<CooleWebappApi.IWeatherForecast[]>;

  constructor(
    private readonly weatherForecastClient: CooleWebappApi.WeatherForecastClient,
  ) {
    this.forecasts = weatherForecastClient.get().pipe(
      //shareReplay(1)
    );
  }

  title = 'CooleWebappFrontend';
}
