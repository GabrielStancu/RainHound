import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { AlertModel } from '../models/alert.model';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {

  constructor(private httpClient: HttpClient) { }

  public getWeather(city: string) : Observable<object> {
    return this.httpClient.get<object>(environment.weatherUrl + 'Weather/weather?city=' + city);
  }

  public getForecast(city: string, days: number) : Observable<object> {
    return this.httpClient.get<object>(environment.weatherUrl + 'Weather/forecast?city=' + city + '&days=' + days);
  }

  public setAlert(alert: AlertModel): Observable<object> {
    return this.httpClient.post<object>(environment.alertsUrl + environment.setAlertEndpoint, alert);
  }
}
