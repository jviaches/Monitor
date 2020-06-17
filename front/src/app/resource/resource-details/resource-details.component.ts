import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IResource } from 'src/app/core/models/resource.model';
import { SelectionOptions } from 'src/app/core/shared/selection-options';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { SeriesOptionsType, YAxisOptions } from 'highcharts';
import { Chart } from 'angular-highcharts';
import * as moment from 'moment-timezone';

@Component({
    selector: 'app-resource-details',
    templateUrl: './resource-details.component.html',
    styleUrls: ['./resource-details.component.scss']
})
export class ResourceDetailsComponent implements OnInit {

    resource: IResource;
    siteFormGroup: FormGroup;
    periodicityOptions = SelectionOptions.periodicityOptions();
    chartMap: Map<number, Chart>;

    constructor(private activatedRoute: ActivatedRoute, private router: Router, private formBuilder: FormBuilder) {
    }

    ngOnInit(): void {
        this.activatedRoute.paramMap.subscribe(params => {
            this.resource = window.history.state.data.selectedResource;
            // if (window === undefined) {
            //     this.router.navigate(['dashboard']);
            // }
            console.log(this.resource);
        });

        this.siteFormGroup = this.formBuilder.group({
            periodicity: [this.resource.monitorPeriod, Validators.required],
        });

        this.buildHistoryStatusChart();
    }

    private buildHistoryStatusChart() {
        this.chartMap = new Map<number, Chart>();

        const historyData: any[] = [];
        // tslint:disable-next-line:max-line-length
        this.resource.history.map(record => historyData.push(({ name: moment(record.requestDate).format('LLL'), y: Number(record.result) })));

        this.chartMap[this.resource.id] = new Chart({
            chart: {
                type: 'spline',
                scrollablePlotArea: {
                    minWidth: 600,
                    scrollPositionX: 1
                }
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
                    to: 511,
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
                name: this.resource.url,
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
    }
}
