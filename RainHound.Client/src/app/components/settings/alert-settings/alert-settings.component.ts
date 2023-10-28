import { Component } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-alert-settings',
  templateUrl: './alert-settings.component.html',
  styleUrls: ['./alert-settings.component.css']
})
export class AlertSettingsComponent {
  public alertSettingsForm: FormGroup = new FormGroup({});

  public constructor(private toastr: ToastrService) {}

  ngOnInit() {
    const minTemp = Number(localStorage.getItem('rainhound-min-temp') ?? '0');
    const maxTemp = Number(localStorage.getItem('rainhound-max-temp') ?? '50');
    const chancesOfRain = Number(localStorage.getItem('rainhound-chances-of-rain') ?? '50');
    const email = localStorage.getItem('rainhound-email') ?? '';

    this.alertSettingsForm = new FormGroup({
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
