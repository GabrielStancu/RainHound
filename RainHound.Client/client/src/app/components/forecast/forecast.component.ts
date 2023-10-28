import { Component } from '@angular/core';
import { ForecastModel } from 'src/app/models/forecast.model';
import { WeatherService } from 'src/app/services/weather.service.ts.service';
import { ForecastMapper } from 'src/app/utils/mappers/forecast.mapper';

@Component({
  selector: 'app-forecast',
  templateUrl: './forecast.component.html',
  styleUrls: ['./forecast.component.css']
})
export class ForecastComponent {
  public forecast: ForecastModel = new ForecastModel();

  constructor (private weatherService: WeatherService) { }

  ngOnInit() {
    const city = localStorage.getItem('rainhound-city') ?? 'London';
    const forecastDays = Number(localStorage.getItem('rainhound-forecast-days') ?? '1') ?? 1;

    this.weatherService.getForecast(city, forecastDays).subscribe(resp => {
      this.forecast = ForecastMapper.map(resp);
    }, error => {
      console.log('ERROR: ' +  JSON.stringify(error));
    })
  }
}
