import { Component } from '@angular/core';
import { WeatherService } from 'src/app/services/weather.service.ts.service';

@Component({
  selector: 'app-forecast',
  templateUrl: './forecast.component.html',
  styleUrls: ['./forecast.component.css']
})
export class ForecastComponent {
  // Forecast data (you can replace this with your actual data)
  forecastData = [
    { name: 'Next Hour', temperature: 22, wind: 12, humidity: 45 },
    { name: 'Next 2 Hours', temperature: 23, wind: 15, humidity: 40 }
    // Add more forecast data as needed
  ];

  constructor (private weatherService: WeatherService) { }

  convertToCelsius(fahrenheit: number): number {
    return ((fahrenheit - 32) * 5) / 9;
  }

  ngOnInit() {
    this.weatherService.getForecast().subscribe(resp => {
      console.log('Forecast:' + JSON.stringify(resp));
    }, error => {
      console.log('ERROR: ' +  JSON.stringify(error));
    })
  }
}
