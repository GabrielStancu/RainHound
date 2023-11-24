import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { MonitoringService } from 'src/app/services/monitoring.service';
import { WeatherService } from 'src/app/services/weather.service';
import { ForecastMapper } from 'src/app/utils/mappers/forecast.mapper';

@Component({
  selector: 'app-forecast',
  templateUrl: './forecast.component.html',
  styleUrls: ['./forecast.component.css']
})
export class ForecastComponent {
  public labels: string[] = [];
  public temperature: number[] = [];
  public precipitation: number[] = [];
  public humidity: number[] = [];
  public chanceOfRain: number[] = [];

  public city: string = "";
  public forecastDays : number = 1;

  constructor (private weatherService: WeatherService, private monitoringService: MonitoringService, private toastr: ToastrService) { }

  ngOnInit() {
    this.city = localStorage.getItem('rainhound-city') ?? 'London';
    this.forecastDays = Number(localStorage.getItem('rainhound-forecast-days') ?? '1') ?? 1;

    this.monitoringService.logTrace("Getting weather for city: {city} for days: {forecastDays}", { city: this.city, forecastDays: this.forecastDays });
    this.weatherService.getForecast(this.city, this.forecastDays).subscribe(resp => {
      const forecast = ForecastMapper.map(resp);

      this.labels = forecast.hours.map(f => f.time);
      this.temperature = forecast.hours.map(f => f.tempC);
      this.precipitation = forecast.hours.map(f => f.precipMm);
      this.humidity = forecast.hours.map(f => f.humidity);
      this.chanceOfRain = forecast.hours.map(f => f.chanceOfRain);

      this.monitoringService.logTrace("Successfully fetched forecast", {});
    }, error => {
      this.monitoringService.logException(error);
      this.toastr.error("Could not fetch forecast, please try again later!");
    })
  }
}
