import { ResourceService } from './../core/services/resource.service';
import { Component, OnInit } from '@angular/core';
import { ThemePalette } from '@angular/material/core';
import { IResource } from '../core/models/resource.model';
import { Chart } from 'angular-highcharts';
import { Options, PointOptionsObject, Point } from 'highcharts';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  resources: IResource[] = [];
  activeResources: IResource[] = [];
  inActiveResources: IResource[] = [];

  chartMap: Map<number, Chart>;

  panelOpenState = false;

  // options
  // showXAxis = true;
  // showYAxis = true;
  // gradient = false;
  // showLegend = false;
  // showXAxisLabel = true;
  // xAxisLabel = 'Date';
  // showYAxisLabel = true;
  // yAxisLabel = 'Status Code';
  // timeline = false;
  // autoScale = true;
  // options: Options;

  // color: ThemePalette = 'accent';
  // checked = true;
  // disabled = false;

  // chartOptions: any;

  constructor(private resourceService: ResourceService) {
  }

  ngOnInit(): void {
    this.resourceService.getResources(1).subscribe(resource => {
      this.resources = resource;
      this.setResourceStatus(resource);
      this.buildHistoryStatusChart(resource);
      this.activeResources = this.resources.filter(res => res.isMonitorActivated);
      this.inActiveResources = this.resources.filter(res => !res.isMonitorActivated);
    });
  }

  setResourceStatus(resources: IResource[]) {
    resources.map(record => {
      const lastResponseDate = new Date(Math.max.apply(null, record.history.map(e => {
        return new Date(e.responseDate);
      })));
      record.status = record.history.filter( item => new Date(item.responseDate).getTime() === lastResponseDate.getTime())[0].result;
    });
  }

  onSelect(event) {
    console.log(event);
  }

  private buildHistoryStatusChart(resources: IResource[]) {
    this.chartMap = new Map<number, Chart>();

    resources.forEach(element => {

      const historyData: any[] = [];
      element.history.map(record => historyData.push( ({ name: record.responseDate, y: Number(record.result) })));

      this.chartMap[element.id] = new Chart({
        chart: {
          type: 'line',
        },
        title: {
          text: 'Monitoring History'
        },
        // subtitle: {
        //   text: 'Period: Jan 2019 - Dec 2019'
        // },
        credits: {
          enabled: false
        },
        series: [{
          type: 'line',
          name: element.url,
          data: historyData,
          color: '#FF0000'
        }],
        xAxis: {
          title: {
            text: 'Timeline'
          },
          categories: historyData.map( cat => cat.name),
        },
        yAxis: {
          title: {
            text: 'Status Code'
          },
        },
      });
    });
  }
}
