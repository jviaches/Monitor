import { Component, OnInit } from '@angular/core';
import { IResource } from 'src/app/core/models/resource.model';
import { Chart } from 'angular-highcharts';
import { SelectionOptions } from 'src/app/core/shared/selection-options';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ResourceService } from 'src/app/core/services/resource.service';
import { GeneralService } from 'src/app/core/services/general.service';
import { AuthorizationService } from 'src/app/core/services/authentication.service';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
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
            this.chartMap = this.resourceService.buildHistoryStatusChart(this.resources);
            this.resourceService.resetMonitoringIntervalRefresh(this.resources);

            modalRef.close();
        });
    }

    getResources() {
        this.resourceService.getResources().subscribe(resource => {
            this.resources = resource;
            this.chartMap = this.resourceService.buildHistoryStatusChart(this.resources);

            this.resourceService.resetMonitoringIntervalRefresh(this.resources);
        });
    }

    monitorChange(event: MatSlideToggleChange, resource: IResource) {
        resource.isMonitorActivated = event.checked;

        this.generalService.showYesNoModalMessage().subscribe(data => {
            if (data === 'yes') {
                this.resourceService.updateResource(resource).subscribe(() => {
                    this.resourceService.getResources().subscribe(() => {
                        this.resourceService.resetMonitoringIntervalRefresh(this.resources);
                        this.generalService.showActionConfirmationSuccess(`Resource successfully updated!`);
                    });
                });
            }
        });
    }

    addResource() {
        const resource = {
            url: this.getUrl.value.replace(/\/$/, '') + '',
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
