import { ResourceService } from './../core/services/resource.service';
import { Component, OnInit } from '@angular/core';
import { IResource } from '../core/models/resource.model';
import { Chart } from 'angular-highcharts';
import { MatDialogConfig, MatDialog } from '@angular/material/dialog';
import { ResourceAddComponent } from '../resources/resource-add/resource-add.component';
import { ResourceEditComponent } from '../resources/resource-edit/resource-edit.component';
import { GeneralService } from '../core/services/general.service';
import { AuthorizationService } from '../core/services/authentication.service';

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

  constructor(private resourceService: ResourceService, public dialog: MatDialog,
              private generalService: GeneralService, public authService: AuthorizationService) {
  }

  ngOnInit(): void {
    this.getResources();
  }

  getResources() {
    this.resourceService.getResources().subscribe(resource => {
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
        return new Date(e.requestDate);
      })));

      if (record.history.length > 0) {
        record.status = record.history.filter(item => new Date(item.requestDate).getTime() === lastResponseDate.getTime())[0].result;
      } else {
        record.status = '000';
      }
    });
  }

  onSelect(event) {
  }

  private buildHistoryStatusChart(resources: IResource[]) {
    this.chartMap = new Map<number, Chart>();

    resources.forEach(element => {

      const historyData: any[] = [];
      element.history.map(record => historyData.push(({ name: record.requestDate, y: Number(record.result) })));

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
          categories: historyData.map(cat => cat.name),
        },
        yAxis: {
          title: {
            text: 'Status Code'
          },
        },
      });
    });
  }

  addResource() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.data = { data: '' };
    dialogConfig.disableClose = true;
    dialogConfig.panelClass = 'custom-modal-dialog-transparent-background';
    const dialogRef = this.dialog.open(ResourceAddComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(() => {
      this.getResources();
    });
  }

  editResource(resource: IResource) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.data = { data: resource };
    dialogConfig.disableClose = true;
    dialogConfig.panelClass = 'custom-modal-dialog-transparent-background';
    const dialogRef = this.dialog.open(ResourceEditComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(() => {
      this.getResources();
    });
  }

  deleteResource(resource: IResource) {

    this.generalService.showYesNoModalMessage().subscribe(data => {
      if (data === 'yes') {
        this.resourceService.deleteResource(resource).subscribe(() => {
          this.generalService.showActionConfirmation(`Resource: ${resource.url} successfully deleted`);
          this.getResources();
        });
      }
    });
  }

  logOff() {
    this.generalService.showYesNoModalMessage().subscribe( data => {
      if (data === 'yes') {
        this.authService.logOut();
        window.location.reload();
      }
    });
  }
}
