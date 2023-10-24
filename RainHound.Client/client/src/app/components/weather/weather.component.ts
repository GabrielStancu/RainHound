import { Component } from '@angular/core';
import { WeatherModel } from 'src/app/models/weather.model';
import { WeatherService } from 'src/app/services/weather.service.ts.service';
import { WeatherMapper } from 'src/app/utils/mappers/weather.mapper';

@Component({
  selector: 'app-weather',
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.css']
})
export class WeatherComponent {
  public weather: WeatherModel = new WeatherModel();
  public city = "";

  constructor(private weatherService: WeatherService) {}

  convertToCelsius(fahrenheit: number): number {
    return ((fahrenheit - 32) * 5) / 9;
  }

  ngOnInit() {
    this.city = localStorage.getItem('rainhound-city') ?? 'London';

    this.weatherService.getWeather(this.city).subscribe(resp => {
      this.weather = WeatherMapper.map(resp);
    }, error => {
      console.log('ERROR: ' +  JSON.stringify(error));
    })
  }
}
