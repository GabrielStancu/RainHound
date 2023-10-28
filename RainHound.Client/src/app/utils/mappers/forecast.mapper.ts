import { ForecastModel } from "src/app/models/forecast.model";
import { HourModel } from "src/app/models/hour.model";

export class ForecastMapper {
  public static map(forecastResponse: any): ForecastModel {
    const forecastModel = new ForecastModel();

    forecastResponse.forecast.forecastday.forEach((forecastDay: any) => {
      const hourModels = forecastDay.hour.map((hour: any) => {
        const hourModel = new HourModel();

        hourModel.time = hour.time;
        hourModel.tempC = hour.temp_c;
        hourModel.tempF = hour.temp_f;
        hourModel.condition = hour.condition.text;
        hourModel.windKph = hour.wind_kph;
        hourModel.windMph = hour.wind_mph;
        hourModel.pressureMb = hour.pressure_mb;
        hourModel.pressureIn = hour.pressure_in;
        hourModel.precipMm = hour.precip_mm;
        hourModel.precipIn = hour.precip_in;
        hourModel.humidity = hour.humidity;
        hourModel.cloud = hour.cloud;
        hourModel.willRain = hour.will_rain == 0 ? false : true;
        hourModel.chanceOfRain = hour.chance_of_rain;

        return hourModel;
      });

      forecastModel.hours.push(...hourModels);
    });

    return forecastModel;
  }
}
