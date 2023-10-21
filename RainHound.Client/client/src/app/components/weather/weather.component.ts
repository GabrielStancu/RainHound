import { Component } from '@angular/core';
import { WeatherService } from 'src/app/services/weather.service.ts.service';

@Component({
  selector: 'app-weather',
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.css']
})
export class WeatherComponent {
  currentTemperature: number = 20;
  currentWind: number = 10;
  currentHumidity: number = 50;

  constructor(private weatherService: WeatherService) {}

  convertToCelsius(fahrenheit: number): number {
    return ((fahrenheit - 32) * 5) / 9;
  }

  ngOnInit() {
    this.weatherService.getWeather().subscribe(resp => {
      console.log('Weather:' + resp);
    }, error => {
      console.log('ERROR: ' +  error.message);
    })
  }
}
