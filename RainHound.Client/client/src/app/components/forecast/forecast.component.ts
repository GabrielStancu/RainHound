import { Component } from '@angular/core';
import { ChartConfiguration, ChartOptions } from 'chart.js';
import { ForecastModel } from 'src/app/models/forecast.model';
import { WeatherService } from 'src/app/services/weather.service.ts.service';
import { ForecastMapper } from 'src/app/utils/mappers/forecast.mapper';

@Component({
  selector: 'app-forecast',
  templateUrl: './forecast.component.html',
  styleUrls: ['./forecast.component.css']
})
export class ForecastComponent {
  constructor (private weatherService: WeatherService) { }

  ngOnInit() {
    const city = localStorage.getItem('rainhound-city') ?? 'London';
    const forecastDays = Number(localStorage.getItem('rainhound-forecast-days') ?? '1') ?? 1;

    this.weatherService.getForecast(city, forecastDays).subscribe(resp => {
      const forecast = ForecastMapper.map(resp);
      this.lineChartData = {
        labels: forecast.hours.map(hour => hour.time),
        datasets: [
          {
            data: forecast.hours.map(hour => hour.tempC),
            label: 'Temperature ' + city + ' (' + forecastDays + ' days)',
            fill: true,
            tension: 0.5,
            borderColor: 'black',
            backgroundColor: 'rgba(255,0,0,0.3)'
          }
        ]
      }
    }, error => {
      console.log('ERROR: ' +  JSON.stringify(error));
    })
  }

  public lineChartData: ChartConfiguration<'line'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        label: '',
        fill: true,
        tension: 0.5,
        borderColor: 'black',
        backgroundColor: 'rgba(255,0,0,0.3)'
      }
    ]
  };
  public lineChartOptions: ChartOptions<'line'> = {
    responsive: true
  };
  public lineChartLegend = true;
}
