import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WeatherComponent } from './components/weather/weather.component';
import { ForecastComponent } from './components/forecast/forecast.component';
import { SettingsComponent } from './components/settings/settings.component';

const routes: Routes = [
  { path: 'weather', component: WeatherComponent },
  { path: 'forecast', component: ForecastComponent },
  { path: 'settings', component: SettingsComponent },
  { path: '', redirectTo: '/weather', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
