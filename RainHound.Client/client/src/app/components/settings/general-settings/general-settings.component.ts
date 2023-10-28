import { Component, Input } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-general-settings',
  templateUrl: './general-settings.component.html',
  styleUrls: ['./general-settings.component.css']
})
export class GeneralSettingsComponent {
  public generalSettingsForm: FormGroup = new FormGroup({});

  public constructor(private toastr: ToastrService) {}

  ngOnInit() {
    const city = localStorage.getItem('rainhound-city') ?? '';
    const forecastDays = Number(localStorage.getItem('rainhound-forecast-days') ?? '1');
    const email = localStorage.getItem('rainhound-email') ?? '';

    this.generalSettingsForm = new FormGroup({
      forecastDays: new FormControl(forecastDays, [Validators.required, Validators.min(1), Validators.max(3)]),
      city: new FormControl(city, [Validators.required]),
      email: new FormControl(email, [Validators.required, Validators.email])
    });
  }

  public saveSettings = (settingsFormValue: any) => {
    if (this.generalSettingsForm.valid) {
      this.submitSettings(settingsFormValue);
    } else {
      this.toastr.error("Invalid settings!");
    }
  }

  private submitSettings = (settingsFormValue: any) => {
    localStorage.setItem('rainhound-city', settingsFormValue.city);
    localStorage.setItem('rainhound-forecast-days', settingsFormValue.forecastDays);
    localStorage.setItem('rainhound-email', settingsFormValue.email);

    this.toastr.success("Settings Saved!");
  }

  public hasError = (controlName: string, errorName: string) =>{
    return this.generalSettingsForm.controls[controlName].hasError(errorName);
  }
}
