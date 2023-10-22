import { WeatherModel } from "src/app/models/weather.model";

export class WeatherMapper {
  public static map(weatherResponse: any): WeatherModel {
    const weatherModel = new WeatherModel();

    weatherModel.tempC = weatherResponse.current.temp_c;
    weatherModel.tempF = weatherResponse.current.temp_f;
    weatherModel.text = weatherResponse.current.condition.text;
    weatherModel.windMph = weatherResponse.current.wind_mph;
    weatherModel.windKph = weatherResponse.current.wind_kph;
    weatherModel.pressureMb = weatherResponse.current.pressure_mb;
    weatherModel.pressureIn = weatherResponse.current.pressure_in;
    weatherModel.precipMm = weatherResponse.current.precip_mm;
    weatherModel.precipIn = weatherResponse.current.precip_in;
    weatherModel.humidity = weatherResponse.current.humidity;
    weatherModel.feelsLikeC = weatherResponse.current.feelslike_c;
    weatherModel.feelsLikeF = weatherResponse.current.feelslike_f;

    return weatherModel;
  }
}
