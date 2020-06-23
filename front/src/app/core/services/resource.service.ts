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

    constructor(private httpClient: HttpClient, private generalService: GeneralService, private authService: AuthorizationService) { }

    buildHistoryStatusChart(resources: IResource[]): Map<number, Chart> {
        const chartMap = new Map<number, Chart>();

        resources.forEach(element => {

            const historyData: any[] = [];
            element.history.map(record => historyData.push(({ name: moment(record.requestDate).format('LLL'), y: Number(record.result) })));

            chartMap[element.id] = new Chart({
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
                } as unknown as SeriesOptionsType],
                navigation: {
                    menuItemStyle: {
                        fontSize: '10px'
                    }
                },
            });
        });
        return chartMap;
    }

    resetMonitoringIntervalRefresh(resources: IResource[]) {
        // pick the most periodic and aply minimal refresh rate.
        clearInterval(this.interval);

        if (resources.length === 0) {
            return;
        }

        const minimalRefreshRate = Math.min(...resources.filter(res => res.isMonitorActivated)
            .map(resource => resource.monitorPeriod));
        if (minimalRefreshRate > 0 && minimalRefreshRate < 1000) { // to prevent infinity --> all resources are disabled
            this.interval = setInterval(() => {
                this.getResources();
            }, minimalRefreshRate * 60000); // millisec
        } else {
            clearInterval(this.interval);
        }
    }

    getResources(): Observable<IResource[]> {
        return this.httpClient.get<IResource[]>(this.generalService.URL + `Resources/GetByUserId/${this.authService.getUserName()}`);
    }

    addResource(resource: any): Observable<any> {
        return this.httpClient.post<any>(this.generalService.URL + 'Resources', resource);
    }

    updateResource(resource: any): Observable<any> {
        return this.httpClient.post<any>(this.generalService.URL + 'Resources/Update', resource);
    }

    deleteResource(id: number): Observable<any> {
        return this.httpClient.delete(this.generalService.URL + `Resources/Delete/${id}`);
    }
}
