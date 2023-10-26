import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-general-settings',
  templateUrl: './general-settings.component.html',
  styleUrls: ['./general-settings.component.css']
})
export class GeneralSettingsComponent {
  public city = "";
  public forecastDays : number = 1;
  public email = "";

  public submitSettings() {
    if (this.city != "") {
      localStorage.setItem('rainhound-city', this.city);
    }

    if (this.forecastDays > 0) {
      localStorage.setItem('rainhound-forecast-days', this.forecastDays.toString());
    }

    if (this.email != "") {
      localStorage.setItem('rainhound-email', this.email);
    }

    alert("settings saved!");
  }
}
