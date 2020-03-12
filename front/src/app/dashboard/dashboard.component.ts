import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  title = 'front';
  panelOpenState = false;

  view: any[] = [900, 400];

  public multi = [
    {
      name: 'Amazon',
      series: [
        {
          name: 5,
          value: 2650
        },
        {
          name: 10,
          value: 2800      },
        {
          name: 15,
          value: 2000
        }
      ]
    },
    {
      name: 'Google',
      series: [
        {
          name: 5,
          value: 2500
        },
        {
          name: 10,
          value: 3100
        },
        {
          name: 15,
          value: 2350
        }
      ]
    }
  ];

  // options
  showXAxis = true;
  showYAxis = true;
  gradient = false;
  showLegend = true;
  showXAxisLabel = true;
  xAxisLabel = 'Date';
  showYAxisLabel = true;
  yAxisLabel = 'Status Code';
  timeline = false;
  autoScale = true;

  colorScheme = {
    domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA']
  };

  constructor() {
  }

  ngOnInit(): void {
  }

  onSelect(event) {
    console.log(event);
  }

}

