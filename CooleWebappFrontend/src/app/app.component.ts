import { Component } from '@angular/core';
import { CooleWebappApi } from '../generated/coole-webapp-api';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public forecasts?: CooleWebappApi.IWeatherForecast[];

  constructor(private readonly weatherForecastClient: CooleWebappApi.WeatherForecastClient) {
    weatherForecastClient.get().subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }

  title = 'CooleWebappFrontend';
}
