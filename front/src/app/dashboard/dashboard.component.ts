import { ResourceService } from './../core/services/resource.service';
import { Component, OnInit } from '@angular/core';
import { IResource } from '../core/models/resource.model';
import { Chart } from 'angular-highcharts';
import { MatDialogConfig, MatDialog } from '@angular/material/dialog';
import { GeneralService } from '../core/services/general.service';
import { AuthorizationService } from '../core/services/authentication.service';
import * as moment from 'moment-timezone';
import { UserChangePasswordComponent } from '../user/user-change-password/user-change-password.component';
import { YAxisLabelsOptions } from 'highcharts';
import { SelectionOptions } from '../core/shared/selection-options';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  resources: IResource[] = [];
  chartMap: Map<number, Chart>;
  periodicityOptions = SelectionOptions.periodicityOptions();

  interval = undefined;

  siteFormGroup: FormGroup;
  urlRegex = '^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$';

  constructor(private resourceService: ResourceService, public dialog: MatDialog, private formBuilder: FormBuilder,
              private generalService: GeneralService, public authService: AuthorizationService, ) {

    this.siteFormGroup = this.formBuilder.group({
      url: ['', [Validators.required, Validators.pattern(this.urlRegex)]],
      periodicity: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.getInitialResources();
  }

  getInitialResources() {
    const modalRef = this.generalService.showLoadingModal('Fetching data..');
    this.resourceService.getResources().subscribe(resource => {
      this.resources = resource;
      this.buildHistoryStatusChart(resource);
      this.resetMonitoringIntervalRefresh();

      modalRef.close();
    });
  }

  getResources() {
    this.resourceService.getResources().subscribe(resource => {
      this.resources = resource;
      this.buildHistoryStatusChart(resource);

      this.resetMonitoringIntervalRefresh();
    });
  }

  monitorChange(event: MatSlideToggleChange, resource: IResource) {
    resource.isMonitorActivated = event.checked;

    this.generalService.showYesNoModalMessage().subscribe(data => {
      if (data === 'yes') {
        this.resourceService.updateResource(resource).subscribe(() => {
          this.resourceService.getResources().subscribe(() => {
            this.resetMonitoringIntervalRefresh();
            this.generalService.showActionConfirmationSuccess(`Resource successfully updated!`);
          });
        });
      }
    });
  }

  resetMonitoringIntervalRefresh() {
    // pick the most periodic and aply minimal refresh rate.
    clearInterval(this.interval);

    if (this.resources.length === 0) {
      return;
    }

    // tslint:disable-next-line:no-shadowed-variable
    const minimalRefreshRate = Math.min(...this.resources.filter(res => res.isMonitorActivated).map(resource => resource.monitorPeriod));
    if (minimalRefreshRate > 0 && minimalRefreshRate < 1000) { // to prevent infinity --> all resources are disabled
      this.interval = setInterval(() => {
        this.getResources();
      }, minimalRefreshRate * 60000); // millisec
    } else {
      clearInterval(this.interval);
    }
  }

  private buildHistoryStatusChart(resources: IResource[]) {
    this.chartMap = new Map<number, Chart>();

    resources.forEach(element => {

      const historyData: any[] = [];
      element.history.map(record => historyData.push(({ name: moment(record.requestDate).format('LLL'), y: Number(record.result) })));

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
        rangeSelector: {
          enabled: false
        },
        plotOptions: {
          line: {
            dataLabels: {
              enabled: true,
              formatter() {
                if (this.y % 100 !== 0) { // show labels when not equals to 100s
                  return this.y;
                }
              }
            },
            enableMouseTracking: true,
          },
        },
        series: [{
          type: 'line',
          showInLegend: false,
          name: element.url,
          data: historyData,
          color: '#007bff'
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
          labels: {
            formatter() {
              if (historyData.map(cat => cat.y).includes(this.value)) {
                return this.value;
              }
            }
          } as unknown as YAxisLabelsOptions,
          range: 500
        },
      });
    });
  }

  addResource() {
    const resource = {
      url: this.getUrl.value + '',
      userId: this.authService.getUserName(),
      monitorPeriod: this.getPeriodicity.value,
      isMonitorActivated: true
    };

    this.resourceService.addResource(resource).subscribe(() => {
      this.generalService.showActionConfirmationSuccess('New resource succesfully created');
      this.clearFormGroup(this.siteFormGroup);
      this.getResources();
    });
  }

  deleteResource(resource: IResource) {

    this.generalService.showYesNoModalMessage().subscribe(data => {
      if (data === 'yes') {
        this.resourceService.deleteResource(resource.id).subscribe(() => {
          this.generalService.showActionConfirmationSuccess(`Resource: ${resource.url} successfully deleted`);
          this.getResources();
        });
      }
    });
  }

  logOff() {
    this.generalService.showYesNoModalMessage().subscribe(data => {
      if (data === 'yes') {
        this.authService.logOut();
      }
    });
  }

  changePassword() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.disableClose = true;
    dialogConfig.panelClass = 'custom-modal-dialog-transparent-background';
    this.dialog.open(UserChangePasswordComponent, dialogConfig);
  }

  clearFormGroup(formGroup: FormGroup) {
    formGroup.reset();

    Object.keys(formGroup.controls).forEach(key => {
      formGroup.get(key).setErrors(null);
    });
  }

  get getUrl() {
    return this.siteFormGroup.get('url');
  }

  get getPeriodicity() {
    return this.siteFormGroup.get('periodicity');
  }
}
