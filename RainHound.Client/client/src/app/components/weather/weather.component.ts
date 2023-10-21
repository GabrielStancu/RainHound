import { Component } from '@angular/core';

@Component({
  selector: 'app-weather',
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.css']
})
export class WeatherComponent {
  selectedTab: 'current' | 'forecast' = 'current'; // Default to 'current'

  // Current weather data
  currentTemperature: number = 20;
  currentWind: number = 10;
  currentHumidity: number = 50;

  // Forecast data (you can replace this with your actual data)
  forecastData = [
    { name: 'Next Hour', temperature: 22, wind: 12, humidity: 45 },
    { name: 'Next 2 Hours', temperature: 23, wind: 15, humidity: 40 }
    // Add more forecast data as needed
  ];

  convertToCelsius(fahrenheit: number): number {
    return ((fahrenheit - 32) * 5) / 9;
  }

  showTab(tab: 'current' | 'forecast'): void {
    this.selectedTab = tab;
  }
}
