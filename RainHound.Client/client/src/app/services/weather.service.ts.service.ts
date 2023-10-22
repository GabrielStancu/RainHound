import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {

  constructor(private httpClient: HttpClient) { }

  public getWeather() : Observable<object> {
    const city = 'Cluj-Napoca'; // should be taken from localStorage
    return this.httpClient.get<object>(environment.apiUrl + '/Weather/weather?city=' + city);
  }

  public getForecast() : Observable<object> {
    const city = 'Cluj-Napoca'; // should be taken from localStorage
    const days = 2; // should be taken from local storage
    return this.httpClient.get<object>(environment.apiUrl + '/Weather/forecast?city=' + city + '&days=' + days);
  }
}
