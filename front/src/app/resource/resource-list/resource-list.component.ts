import { Component, OnInit } from '@angular/core';
import { IResource } from 'src/app/core/models/resource.model';
import { SelectionOptions } from 'src/app/core/shared/selection-options';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ResourceService } from 'src/app/core/services/resource.service';
import { GeneralService } from 'src/app/core/services/general.service';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { Router } from '@angular/router';

@Component({
    selector: 'app-resource-list',
    templateUrl: './resource-list.component.html',
    styleUrls: ['./resource-list.component.scss']
})
export class ResourceListComponent implements OnInit {
    periodicityOptions = SelectionOptions.periodicityOptions();
    addSiteFormGroup: FormGroup;
    updateSiteFormGroup: FormGroup;
    urlRegex = '^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$';

    constructor(public resourceService: ResourceService, private formBuilder: FormBuilder, private router: Router,
                private generalService: GeneralService, public authService: AuthenticationService) {

        this.addSiteFormGroup = this.formBuilder.group({
            url: ['', [Validators.required, Validators.pattern(this.urlRegex)]],
            periodicity: ['', Validators.required],
        });

        this.updateSiteFormGroup = this.formBuilder.group({
            enabled: ['', [Validators.required]],
            emailAlert: ['', Validators.required],
            slackAlert: ['', Validators.required],
            slackChannel: ['', Validators.required],
        });
    }

    ngOnInit(): void {
    }

    monitorActivityChange(event: MatSlideToggleChange, resource: IResource) {
        resource.monitorItem.isActive = event.checked;
    }

    monitorEmailNotification(event: MatSlideToggleChange, resource: IResource) {
        resource.communicationChanel.notifyByEmail = event.checked;
    }

    monitorSlackNotification(event: MatSlideToggleChange, resource: IResource) {
        resource.communicationChanel.notifyBySlack = event.checked;
    }


    addResource() {
        const resource = {
            url: this.getUrl.value.replace(/\/$/, '') + '',
            userId: this.authService.currentUserValue.id,
            periodicity: this.getPeriodicity.value,
            monitorActivated: true
        };

        this.resourceService.addResource(resource);
    }

    updateResource(resource: IResource) {
        this.resourceService.updateResource(resource);
    }

    deleteResource(resource: IResource) {

        this.generalService.showYesNoModalMessage().subscribe(data => {
            if (data === 'yes') {
                this.resourceService.deleteResource(resource.id);
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
        return this.addSiteFormGroup.get('url');
    }

    get getPeriodicity() {
        return this.addSiteFormGroup.get('periodicity');
    }

    redirectToResourceDetails(selectedResource: IResource) {
        this.router.navigate([`dashboard/resource-details/${selectedResource.id}`], { state: { data: { selectedResource } } });
    }
}
