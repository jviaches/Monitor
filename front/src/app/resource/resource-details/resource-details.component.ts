import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IResource } from 'src/app/core/models/resource.model';
import { SelectionOptions } from 'src/app/core/shared/selection-options';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Chart } from 'angular-highcharts';
import { ResourceService } from 'src/app/core/services/resource.service';

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

    constructor(private activatedRoute: ActivatedRoute, private router: Router,
                private formBuilder: FormBuilder, private resourceService: ResourceService) {
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

        const resources = [this.resource];
        this.chartMap = this.resourceService.buildHistoryStatusChart(resources);
    }
}
