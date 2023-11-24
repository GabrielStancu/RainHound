import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { WeatherModel } from 'src/app/models/weather.model';
import { MonitoringService } from 'src/app/services/monitoring.service';
import { WeatherService } from 'src/app/services/weather.service';
import { WeatherToImageConverter } from 'src/app/utils/convertors/weather-to-image.converter';
import { WeatherMapper } from 'src/app/utils/mappers/weather.mapper';

@Component({
  selector: 'app-weather',
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.css']
})
export class WeatherComponent {
  public weather: WeatherModel = new WeatherModel();
  public city = "";

  constructor(private weatherService: WeatherService, private monitoringService: MonitoringService, private toastr: ToastrService) {}

  convertToCelsius(fahrenheit: number): number {
    return ((fahrenheit - 32) * 5) / 9;
  }

  ngOnInit() {
    this.city = localStorage.getItem('rainhound-city') ?? 'London';

    this.monitoringService.logTrace("Getting weather for city: {city}", { city: this.city });
    this.weatherService.getWeather(this.city).subscribe(resp => {
      this.weather = WeatherMapper.map(resp);
      this.monitoringService.logTrace("Successfully fetched weather", {});
    }, error => {
      this.monitoringService.logException(error);
      this.toastr.error("Could not fetch weather, please try again later!");
    })
  }

  public convertToWeatherImage(): string {
    return WeatherToImageConverter.convert(this.weather.text);
  }
}
