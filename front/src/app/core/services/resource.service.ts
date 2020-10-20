import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IResource } from '../models/resource.model';
import { GeneralService } from './general.service';
import { SeriesOptionsType, YAxisOptions } from 'highcharts';
import { Chart } from 'angular-highcharts';
import * as moment from 'moment-timezone';
import { AuthenticationService } from './authentication.service';

@Injectable({
    providedIn: 'root'
})
export class ResourceService {

    interval = undefined;
    resources: IResource[] = [];
    chartMap = new Map<number, Chart>();

    constructor(private httpClient: HttpClient, private generalService: GeneralService, private authService: AuthenticationService) {
        const modalRef = this.generalService.showLoadingModal('Fetching data..');
        this.getResources().subscribe(resources => {
            this.resources = resources;
            this.buildHistoryStatusChart();
            this.resetMonitoringIntervalRefresh();
            modalRef.close();
        });
    }

    private buildHistoryStatusChart(): Map<number, Chart> {
        this.chartMap = new Map<number, Chart>();

        this.resources.forEach(element => {

            const historyData: any[] = [];
            element.monitorItem.history
                .map(record => historyData.push(({ name: moment(record.scanDate).format('LLL'), y: Number(record.result) })));

            this.chartMap[element.id] = new Chart({
                chart: {
                    type: 'area'
                },
                title: {
                    text: 'Monitoring History'
                },
                legend: {
                    layout: 'vertical',
                    align: 'left',
                    verticalAlign: 'top',
                    x: 150,
                    y: 100,
                    floating: true,
                    borderWidth: 1,
                    backgroundColor: '#FFFFFF'
                },
                xAxis: {
                    categories: historyData.map(cat => cat.name),
                    type: 'datetime',
                    labels: {
                        overflow: 'justify',
                        rotation: -45,
                    },
                    title: {
                        text: 'Timeline'
                    },
                    gridLineWidth: 0.5,
                    // tickInterval: 20,
                    lineWidth: 2,
                    lineColor: '#92A8CD',
                    tickWidth: 3,
                    tickLength: 6,
                    tickColor: '#92A8CD',
                },
                yAxis: {
                    title: {
                        text: 'Status Code'
                    },
                    min: 200,
                    max: 600,
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
                            text: 'Warning',
                            style: {
                                color: '#ffcdb2'
                            }
                        }
                    }, { // Error
                        from: 400,
                        to: 600,
                        color: 'rgba(68, 170, 213, 0.1)',
                        label: {
                            text: 'Error',
                            style: {
                                color: '#e5989b'
                            }
                        }
                    }]
                } as unknown as YAxisOptions | YAxisOptions[],
                tooltip: {
                    shared: true,
                    valuePrefix: 'Http Code: '
                },
                credits: {
                    enabled: false
                },
                plotOptions: {
                    areaspline: {
                        fillOpacity: 0.5
                    },
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: [{
                    name: element.url,
                    showInLegend: false,
                    data: historyData,
                    // turboThreshold: 10000
                } as unknown as SeriesOptionsType]
            });
        });
        return this.chartMap;
    }

    resetMonitoringIntervalRefresh() {
        // pick the most periodic and aply minimal refresh rate.
        clearInterval(this.interval);

        if (this.resources.length === 0) {
            return;
        }

        let minimalRefreshRate = Math.min(...this.resources.filter(res => res.monitoringActivated && res.monitorItem.isActive)
            .map(resource => resource.monitorItem.period));

        console.log('resetMonitoringIntervalRefresh: minimalRefreshRate = ' + minimalRefreshRate);

        if (minimalRefreshRate > 0 && minimalRefreshRate < 1000) { // to prevent infinity --> all resources are disabled
            this.interval = setInterval(() => {
                this.getResources().subscribe(resources => {

                    // PREPARATION TO partial graph refresh
                    // resources.forEach(element => {
                    //     const foundResource = this.resources.find(res => res.id === element.id);
                    //     // tslint:disable-next-line:max-line-length
                    // tslint:disable-next-line:max-line-length
                    //  const lastMonitoredDate = foundResource.history.filter(res => Math.max(new Date(res.requestDate).getMilliseconds()))[0];
                    //  const newStartIndex = element.history.findIndex(history => history.requestDate === lastMonitoredDate.requestDate);

                    //     const newHistoryitems = element.history.slice(newStartIndex, element.history.length - 1);
                    //     if (newHistoryitems.length > 0) {
                    //         console.log(newHistoryitems);
                    //         foundResource.history.concat(newHistoryitems);
                    //     }
                    // });

                    this.resources = resources;
                    minimalRefreshRate = Math.min(...this.resources.filter(res => res.monitoringActivated && res.monitorItem.isActive)
                        .map(resource => resource.monitorItem.period));

                    this.buildHistoryStatusChart();
                });
            }, minimalRefreshRate * 60000); // millisec
        } else {
            clearInterval(this.interval);
        }
    }

    getResources(): Observable<IResource[]> {
        return this.httpClient.get<IResource[]>(this.generalService.URL + `Resources/GetByUserId/${this.authService.currentUserValue.id}`);
    }

    getResourceById(id): Observable<IResource> {
        return this.httpClient.get<IResource>(this.generalService.URL + `Resources/GetById/${id}`);
    }

    addResource(resource: any) {
        this.httpClient.post<any>(this.generalService.URL + 'Resources/Add', resource).subscribe(() => {
            console.log('resource has been added');
            this.getResources().subscribe(resources => {
                this.resources = resources;
                this.buildHistoryStatusChart();
            });
            this.resetMonitoringIntervalRefresh();
        });
    }

    updateResource(resource: IResource) {
        const updateDetails = {
            resourceId: resource.id,
            url: resource.url,
            isMonitorActivate: resource.monitorItem.isActive,
            emalAlert: resource.communicationChanel.notifyByEmail,
            slackAlert: resource.communicationChanel.notifyBySlack,
            slackChannel: resource.communicationChanel.slackChanel
        };

        console.log(updateDetails);

        let headers = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/json; charset=utf-8');

        this.httpClient.put<any>(this.generalService.URL + 'Resources/Update',
                updateDetails, { headers }).subscribe(() => {
                console.log('resource has been updated');

                this.generalService.showActionConfirmationSuccess('Updated!');
                this.getResources().subscribe(resources => {
                    this.resources = resources;
                    this.buildHistoryStatusChart();
                });
                this.resetMonitoringIntervalRefresh();
            });
    }

    deleteResource(id: any) {
        this.httpClient.delete(this.generalService.URL + `Resources/Delete/${id}`).subscribe(() => {
            console.log('resource has been removed');
            this.getResources().subscribe(resources => {
                this.resources = resources;
                this.buildHistoryStatusChart();
            });
            this.resetMonitoringIntervalRefresh();
        });
    }
}
