import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CooleWebappApi } from '../generated/coole-webapp-api';

import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule, HttpClientModule
  ],
  providers: [
    CooleWebappApi.WeatherForecastClient
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
