export class WeatherToImageConverter {
  public static convert(weatherResponse: string): string {
    switch (weatherResponse) {
      case 'Sunny':
        return 'sunny';
      case 'Light rain':
        return 'rain';
      case 'Partly cloudy':
        return 'partly-cloudy';
      case 'Clear':
        return 'sunny';
      case 'Overcast':
        return 'cloudy'
      case 'Cloudy':
        return 'cloudy'
      case 'Patchy rain possible':
        return 'rain';
      default:
        return 'unknown';
    }
  }
}
