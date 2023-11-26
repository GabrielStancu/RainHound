import { Injectable } from "@angular/core";
import { ApplicationInsights } from "@microsoft/applicationinsights-web";
import { environment } from "src/environments/environment";

@Injectable({
  providedIn: 'root'
})
export class MonitoringService {
  appInsights: ApplicationInsights;

  constructor() {
    this.appInsights = new ApplicationInsights({
      config: {
        instrumentationKey: environment.appInsightsInstrumentationKey,
        enableAutoRouteTracking: true
      }
    });

    this.appInsights.loadAppInsights();

    console.log(environment.appInsightsInstrumentationKey);
  }

  logPageView(name?: string, url?: string) {
    this.appInsights.trackPageView({
      name: name,
      uri: url
    });
  }

  logEvent(name: string, properties?: { [key: string]: any}) {
    this.appInsights.trackEvent({name: name}, properties);
  }

  logMetric(name: string, average: number, properties?: { [key: string]: any}) {
    this.appInsights.trackMetric({ name: name, average: average }, properties);
  }

  logException(exception: Error, severityLevel?: number) {
    this.appInsights.trackException({ exception: exception, severityLevel: severityLevel });
  }

  logTrace(message: string, properties: { [key: string]: any}) {
    this.appInsights.trackTrace({ message: message }, properties);
  }
}
