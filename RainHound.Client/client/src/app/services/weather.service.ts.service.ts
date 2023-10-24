import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {

  constructor(private httpClient: HttpClient) { }

  public getWeather(city: string) : Observable<object> {
    return this.httpClient.get<object>(environment.apiUrl + '/Weather/weather?city=' + city);
  }

  public getForecast(city: string, days: number) : Observable<object> {
    return this.httpClient.get<object>(environment.apiUrl + '/Weather/forecast?city=' + city + '&days=' + days);
  }
}
