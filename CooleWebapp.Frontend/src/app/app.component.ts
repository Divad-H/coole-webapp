import { Component } from '@angular/core';
import { CooleWebappApi } from '../generated/coole-webapp-api';
import { AuthService } from './auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public forecasts?: CooleWebappApi.IWeatherForecast[];

  constructor(
    private readonly weatherForecastClient: CooleWebappApi.WeatherForecastClient,
    private readonly authService: AuthService,
  ) {
    authService.login("Karl", "Ratte@1").subscribe(result => {
      console.log(result);
      weatherForecastClient.get().subscribe(result => {
        this.forecasts = result;
      }, error => console.error(error));
    })
  }

  title = 'CooleWebappFrontend';
}
