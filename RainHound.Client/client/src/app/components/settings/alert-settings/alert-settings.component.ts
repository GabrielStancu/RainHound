import { Component } from '@angular/core';

@Component({
  selector: 'app-alert-settings',
  templateUrl: './alert-settings.component.html',
  styleUrls: ['./alert-settings.component.css']
})
export class AlertSettingsComponent {
  public city = "";
  public forecastDays : number = 1;
  public email = "";

  public submitSettings() {}
}
