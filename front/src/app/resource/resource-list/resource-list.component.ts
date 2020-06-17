import { Component, OnInit } from '@angular/core';
import { IResource } from 'src/app/core/models/resource.model';
import { SeriesOptionsType, YAxisOptions } from 'highcharts';
import { Chart } from 'angular-highcharts';
import { SelectionOptions } from 'src/app/core/shared/selection-options';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ResourceService } from 'src/app/core/services/resource.service';
import { GeneralService } from 'src/app/core/services/general.service';
import { AuthorizationService } from 'src/app/core/services/authentication.service';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import * as moment from 'moment-timezone';
import { Router } from '@angular/router';

@Component({
    selector: 'app-resource-list',
    templateUrl: './resource-list.component.html',
    styleUrls: ['./resource-list.component.scss']
})
export class ResourceListComponent implements OnInit {
    resources: IResource[] = [];
    chartMap: Map<number, Chart>;
    periodicityOptions = SelectionOptions.periodicityOptions();

    interval = undefined;

    siteFormGroup: FormGroup;
    urlRegex = '^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$';

    constructor(private resourceService: ResourceService, private formBuilder: FormBuilder, private router: Router,
                private generalService: GeneralService, public authService: AuthorizationService) {

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
        const minimalRefreshRate = Math.min(...this.resources.filter(res => res.isMonitorActivated)
            .map(resource => resource.monitorPeriod));
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
                    type: 'spline',
                    // scrollablePlotArea: {
                    //     minWidth: 600,
                    //     scrollPositionX: 1
                    // }
                },
                title: {
                    text: 'Monitoring History'
                },
                credits: {
                    enabled: false
                },
                xAxis: {
                    categories: historyData.map(cat => cat.name),
                    type: 'datetime',
                    labels: {
                        overflow: 'justify'
                    },
                    title: {
                        text: 'Timeline'
                    },
                },
                yAxis: {
                    title: {
                        text: 'Status Code'
                    },
                    range: 500,
                    minorGridLineWidth: 0,
                    gridLineWidth: 0,
                    alternateGridColor: null,
                    plotBands: [{ // Successful
                        from: 200,
                        to: 299,
                        color: 'rgba(68, 170, 213, 0.1)',
                        label: {
                            text: 'Successful',
                            style: {
                                color: '#606060'
                            }
                        }
                    }, { // Redirection
                        from: 300,
                        to: 399,
                        color: 'rgba(0, 0, 0, 0)',
                        label: {
                            text: 'Redirection',
                            style: {
                                color: '#ffcdb2'
                            }
                        }
                    }, { // Client error
                        from: 400,
                        to: 499,
                        color: 'rgba(68, 170, 213, 0.1)',
                        label: {
                            text: 'Client error',
                            style: {
                                color: '#e5989b'
                            }
                        }
                    }, { // Server error
                        from: 500,
                        to: 599,
                        color: 'rgba(0, 0, 0, 0)',
                        label: {
                            text: 'Server error',
                            style: {
                                color: '#e5989b'
                            }
                        }
                    }]
                } as unknown as YAxisOptions | YAxisOptions[],
                tooltip: {
                    valuePrefix: 'Http Code: '
                },
                plotOptions: {
                    spline: {
                        lineWidth: 4,
                        states: {
                            hover: {
                                lineWidth: 5
                            }
                        },
                        marker: {
                            enabled: false
                        },
                    }
                },
                series: [{
                    name: element.url,
                    showInLegend: false,
                    data: historyData
                } as unknown as SeriesOptionsType],
                navigation: {
                    menuItemStyle: {
                        fontSize: '10px'
                    }
                },
                // labels: {
                //     formatter() {
                //         if (historyData.map(cat => cat.y).includes(this.value)) {
                //             return this.value;
                //         }
                //     }
                // },
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

    redirectToResourceDetails(selectedResource: IResource) {
        this.router.navigate(['dashboard/resource-details'], { state: { data: { selectedResource } } });
    }
}
