import { ResourceService } from './../core/services/resource.service';
import { Component, OnInit } from '@angular/core';
import { ThemePalette } from '@angular/material/core';
import { IResource } from '../core/models/resource.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  resources: IResource[] = [];
  activeResources: IResource[] = [];
  inActiveResources: IResource[] = [];

  panelOpenState = false;

  view: any[] = [900, 360];

  public multi = [
    {
      name: 'Amazon',
      series: [
        {
          name: 'Jan 5, 2020',
          value: 200
        },
        {
          name: 'Jan 6, 2020',
          value: 404      },
        {
          name: 'Jan 7, 2020',
          value: 200
        }
      ]
    },
    {
      name: 'Google',
      series: [
        {
          name: 'Jan 5, 2020',
          value: 400
        },
        {
          name: 'Jan 6, 2020',
          value: 200
        },
        {
          name: 'Jan 7, 2020',
          value: 404
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


  color: ThemePalette = 'accent';
  checked = true;
  disabled = false;

  constructor(private resourceService: ResourceService) {
  }

  ngOnInit(): void {
    this.resourceService.getResources(1).subscribe( resource => {
      this.resources = resource;
      this.activeResources = this.resources.filter(res => res.isMonitorActivated);
      this.inActiveResources = this.resources.filter(res => !res.isMonitorActivated);
    });
  }

  onSelect(event) {
    console.log(event);
  }
}

