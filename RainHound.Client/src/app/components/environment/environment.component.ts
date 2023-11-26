import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-environment',
  templateUrl: './environment.component.html',
  styleUrls: ['./environment.component.css']
})
export class EnvironmentComponent {
  public environment: string = "";
  public weatherUrl: string = "";
  public alertsUrl: string = "";

  constructor() {
    this.environment = environment.environment;
    this.weatherUrl = environment.weatherUrl;
    this.alertsUrl = environment.alertsUrl;
  }
}
