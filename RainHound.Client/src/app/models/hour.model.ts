export class HourModel {
  constructor() {}

  public time: string = '';
  public tempC: number = 0;
  public tempF: number = 0;
  public condition: string = '';
  public windKph: number = 0;
  public windMph: number = 0;
  public pressureMb: number = 0;
  public pressureIn: number = 0;
  public precipMm: number = 0;
  public precipIn: number = 0;
  public humidity: number = 0;
  public cloud: number = 0;
  public willRain: boolean = false;
  public chanceOfRain: number = 0;
}
