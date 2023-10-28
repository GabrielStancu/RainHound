import { Component, Input } from '@angular/core';
import { ChartConfiguration, ChartOptions } from 'chart.js';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-forecast-graph',
  templateUrl: './forecast-graph.component.html',
  styleUrls: ['./forecast-graph.component.css']
})
export class ForecastGraphComponent {
  @Input() public labels: string[] = [];
  @Input() public data: number[] = [];
  @Input() public title: string = "";

  ngOnInit() {
    this.lineChartData = {
      labels: this.labels,
      datasets: [
        {
          data: this.data,
          label: this.title,
          fill: true,
          tension: 0.5,
          borderColor: 'black',
          backgroundColor: 'rgba(61,43,33,0.75)'
        }
      ]
    }
  }

  public lineChartData: ChartConfiguration<'line'>['data'] = {
    labels: [],
    datasets: []
  };
  public lineChartOptions: ChartOptions<'line'> = {
    responsive: true
  };
  public lineChartLegend = true;
}
