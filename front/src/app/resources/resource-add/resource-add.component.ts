import { Component, Inject } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogData } from 'src/app/core/components/modal-dialog/dialog-data';
import { ResourceService } from 'src/app/core/services/resource.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SelectionOptions } from 'src/app/core/shared/selection-options';
import { IResource } from 'src/app/core/models/resource.model';
import { MatSlideToggle } from '@angular/material/slide-toggle';

@Component({
    selector: 'app-root',
    templateUrl: './resource-add.component.html',
    styleUrls: ['./resource-add.component.scss']
})
export class ResourceAddComponent {

    siteFormGroup: FormGroup;
    periodicityOptions = SelectionOptions.periodicityOptions();
    urlRegex = '^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$';

    constructor(public dialog: MatDialog, @Inject(MAT_DIALOG_DATA) { data }: DialogData,
                private resourceService: ResourceService, private formBuilder: FormBuilder) {

        this.siteFormGroup = this.formBuilder.group({
            url: ['', [Validators.required, Validators.pattern(this.urlRegex)]],
            periodicity: ['', Validators.required],
            isActivated: []
        });
    }

    get getUrl() {
        return this.siteFormGroup.get('url');
    }

    get getPeriodicity() {
        return this.siteFormGroup.get('periodicity');
    }

    get getActivationState() {
        return this.siteFormGroup.get('isActivated');
    }

    closeDialog() {
        console.log(this.getActivationState.value);
        this.dialog.closeAll();
    }

    saveWebSiteDialog() {

        const timeOptions = {
            year: 'numeric', month: 'numeric', day: 'numeric',
            hour: 'numeric', minute: 'numeric', second: 'numeric',
            hour12: false
          };

        const resource = {
            url: this.getUrl.value + '',
            userId: '1',
            monitorPeriod: this.getPeriodicity.value,
            isMonitorActivated: this.getActivationState.value === true ? '1' : '0',
            monitorActivationDate: new Intl.DateTimeFormat('en-US', timeOptions).format(new Date()).toString()
        };

        this.resourceService.addResource(resource).subscribe();
        this.closeDialog();
    }
}
