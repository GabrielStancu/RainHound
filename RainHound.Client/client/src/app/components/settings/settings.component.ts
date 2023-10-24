import { Component } from '@angular/core';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent {
  public city = "";
  public forecastDays : number = 1;
  public email = "";

  ngOnInit() {
    this.city = localStorage.getItem('rainhound-city') ?? "";
    this.forecastDays = Number(localStorage.getItem('rainhound-forecast-days') ?? "1");
    this.email = localStorage.getItem('rainhound-email') ?? "";
  }

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
