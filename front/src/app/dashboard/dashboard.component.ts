import { ResourceService } from './../core/services/resource.service';
import { Component, OnInit } from '@angular/core';
import { ThemePalette } from '@angular/material/core';
import { IResource } from '../core/models/resource.model';
import { Chart } from 'angular-highcharts';
import { Options } from 'highcharts';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  resources: IResource[] = [];
  activeResources: IResource[] = [];
  inActiveResources: IResource[] = [];

  charts: Chart[] = [];
  panelOpenState = false;

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
  options: Options;

  color: ThemePalette = 'accent';
  checked = true;
  disabled = false;

  chartOptions: any;

  chart = new Chart({
    chart: {
      type: 'line',
    },
    title: {
      text: 'Monitoring'
    },
    subtitle: {
      text: 'Period: Jan 2019 - Dec 2019'
    },
    credits: {
      enabled: false
    },
    series: [{
      type: 'line',
      name: 'http://www.stack.com',
      data: [{
        name: 'Point 1',
        color: '#00FF00',
        y: 200
      }, {
        name: 'Point 2',
        color: '#FF00FF',
        y: 500
      },
      {
        name: 'Point 2',
        color: '#FF00FF',
        y: 400
      }],
      color: '#FF0000'
    },
    {
      type: 'line',
      name: 'http://www.techflask.com',
      data:
        [{
          name: 'Point 1',
          color: '#A0DB8E',
          y: 400
        }, {
          name: 'Point 2',
          color: '#A0DB8E',
          y: 400
        },
        {
          name: 'Point 2',
          color: '#A0DB8E',
          y: 400
        }],
      color: '#A0DB8E'
    }],
    xAxis: {
      title: {
        text: 'Status'
      },
      // categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
    },
    yAxis: {
      title: {
        text: 'Status'
      },
      //  labels: {
      //   formatter() {
      //     const foundValue =  [200, 400, 404, 500, 504].find(el => el === this.value);
      //     return foundValue ? foundValue + '' : '';
      //   }
      // },
    },
  });

  constructor(private resourceService: ResourceService) {
  }

  ngOnInit(): void {
    this.resourceService.getResources(1).subscribe(resource => {
      this.resources = resource;
      this.activeResources = this.resources.filter(res => res.isMonitorActivated);
      this.inActiveResources = this.resources.filter(res => !res.isMonitorActivated);
    });
  }

  onSelect(event) {
    console.log(event);
  }


}

