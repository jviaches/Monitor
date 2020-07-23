import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IResource } from '../models/resource.model';
import { GeneralService } from './general.service';
import { AuthorizationService } from './authentication.service';
import { SeriesOptionsType, YAxisOptions } from 'highcharts';
import { Chart } from 'angular-highcharts';
import * as moment from 'moment-timezone';

@Injectable({
    providedIn: 'root'
})
export class ResourceService {

    interval = undefined;
    resources: IResource[] = [];
    chartMap = new Map<number, Chart>();

    constructor(private httpClient: HttpClient, private generalService: GeneralService, private authService: AuthorizationService) {
        this.getResources().subscribe(resources => {
            this.resources = resources;
            this.buildHistoryStatusChart();
            this.resetMonitoringIntervalRefresh();
        });
     }

    private buildHistoryStatusChart(): Map<number, Chart> {
        this.chartMap = new Map<number, Chart>();

        this.resources.forEach(element => {

            const historyData: any[] = [];
            element.history.map(record => historyData.push(({ name: moment(record.requestDate).format('LLL'), y: Number(record.result) })));

            this.chartMap[element.id] = new Chart({
                chart: {
                    // renderTo: 'chart',
                    type: 'spline',
                    zoomType: 'x',
                    scrollablePlotArea: {
                        minWidth: 300,
                        scrollPositionX: 1
                    },
                },
                title: {
                    text: 'Monitoring History'
                },
                subtitle: {
                    text: 'Click and drag in the chart to zoom in.'
                },
                credits: {
                    enabled: false
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
                    tickInterval: 20,
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
                    data: historyData,
                    turboThreshold: 10000
                } as unknown as SeriesOptionsType],
                navigation: {
                    menuItemStyle: {
                        fontSize: '10px'
                    }
                },
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

        const minimalRefreshRate = Math.min(...this.resources.filter(res => res.isMonitorActivated)
            .map(resource => resource.monitorPeriod));
        if (minimalRefreshRate > 0 && minimalRefreshRate < 1000) { // to prevent infinity --> all resources are disabled
            this.interval = setInterval(() => {
                this.getResources().subscribe(resources => {

                    // PREPARATION TO partial graph refresh
                    // resources.forEach(element => {
                    //     const foundResource = this.resources.find(res => res.id === element.id);
                    //     // tslint:disable-next-line:max-line-length
                //  const lastMonitoredDate = foundResource.history.filter(res => Math.max(new Date(res.requestDate).getMilliseconds()))[0];
                //  const newStartIndex = element.history.findIndex(history => history.requestDate === lastMonitoredDate.requestDate);

                    //     const newHistoryitems = element.history.slice(newStartIndex, element.history.length - 1);
                    //     if (newHistoryitems.length > 0) {
                    //         console.log(newHistoryitems);
                    //         foundResource.history.concat(newHistoryitems);
                    //     }
                    // });

                    this.resources = resources;
                    this.buildHistoryStatusChart();
                });
            }, minimalRefreshRate * 60000); // millisec
        } else {
            clearInterval(this.interval);
        }
    }

    getResources(): Observable<IResource[]> {
        return this.httpClient.get<IResource[]>(this.generalService.URL + `Resources/GetByUserId/${this.authService.getUserName()}`);
    }

    addResource(resource: any) {
        this.httpClient.post<any>(this.generalService.URL + 'Resources', resource).subscribe(() => {
            this.resetMonitoringIntervalRefresh();
        });
    }

    updateResource(resource: any) {
        this.httpClient.post<any>(this.generalService.URL + 'Resources/Update', resource).subscribe(() => {
            this.resetMonitoringIntervalRefresh();
        });
    }

    deleteResource(id: number) {
        this.httpClient.delete(this.generalService.URL + `Resources/Delete/${id}`).subscribe(() => {
            this.resetMonitoringIntervalRefresh();
        });
    }
}
