import { Component, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-general-settings',
  templateUrl: './general-settings.component.html',
  styleUrls: ['./general-settings.component.css']
})
export class GeneralSettingsComponent {
  public city = "";
  public forecastDays : number = 1;
  public email = "";

  public constructor(private toastr: ToastrService) {}

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

    this.toastr.success("Settings Saved!");
  }
}
