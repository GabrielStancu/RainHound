import { Component } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent {
  public alertSettingsForm: FormGroup = new FormGroup({});

  public constructor(private toastr: ToastrService) {}

  ngOnInit() {
    const city = localStorage.getItem('rainhound-city') ?? '';
    const forecastDays = Number(localStorage.getItem('rainhound-forecast-days') ?? '1');
    const minTemp = Number(localStorage.getItem('rainhound-min-temp') ?? '0');
    const maxTemp = Number(localStorage.getItem('rainhound-max-temp') ?? '50');
    const chancesOfRain = Number(localStorage.getItem('rainhound-chances-of-rain') ?? '50');
    const email = localStorage.getItem('rainhound-email') ?? '';

    this.alertSettingsForm = new FormGroup({
      forecastDays: new FormControl(forecastDays, [Validators.required, Validators.min(1), Validators.max(3)]),
      city: new FormControl(city, [Validators.required]),
      minTemp: new FormControl(minTemp, [this.minTempValidator(maxTemp)]),
      maxTemp: new FormControl(maxTemp, []),
      chancesOfRain: new FormControl(chancesOfRain, [Validators.min(0), Validators.max(100)]),
      email: new FormControl(email, [Validators.required, Validators.email])
    });
  }

  public saveSettings = (settingsFormValue: any) => {
    if (this.alertSettingsForm.valid) {
      this.submitSettings(settingsFormValue);
    } else {
      this.toastr.error("Invalid settings!");
    }
  }

  private submitSettings = (settingsFormValue: any) => {
    localStorage.setItem('rainhound-min-temp', settingsFormValue.minTemp);
    localStorage.setItem('rainhound-max-temp', settingsFormValue.maxTemp);
    localStorage.setItem('rainhound-chances-of-rain', settingsFormValue.chancesOfRain);
    localStorage.setItem('rainhound-email', settingsFormValue.email);
    localStorage.setItem('rainhound-city', settingsFormValue.city);
    localStorage.setItem('rainhound-forecast-days', settingsFormValue.forecastDays);

    this.toastr.success("Settings Saved!");
  }

  public hasError = (controlName: string, errorName: string) =>{
    return this.alertSettingsForm.controls[controlName].hasError(errorName);
  }

  public minTempValidator(maxTemp: number): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const value = control.value;

      if (value === undefined || maxTemp === undefined) {
        return null;
      }

      if (value > maxTemp) {
        return { minTemp: true };
      }

      return null;
    };
  }
}
