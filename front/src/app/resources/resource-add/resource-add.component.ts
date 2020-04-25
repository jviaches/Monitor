import { Component, Inject } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogData } from 'src/app/core/components/modal-dialog/dialog-data';
import { ResourceService } from 'src/app/core/services/resource.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SelectionOptions } from 'src/app/core/shared/selection-options';
import { GeneralService } from 'src/app/core/services/general.service';
import { AuthorizationService } from 'src/app/core/services/authentication.service';

@Component({
    selector: 'app-root',
    templateUrl: './resource-add.component.html',
    styleUrls: ['./resource-add.component.scss']
})
export class ResourceAddComponent {

    siteFormGroup: FormGroup;
    periodicityOptions = SelectionOptions.periodicityOptions();
    urlRegex = '^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$';

    constructor(public dialog: MatDialog, @Inject(MAT_DIALOG_DATA) { data }: DialogData,
                private resourceService: ResourceService, private formBuilder: FormBuilder,
                private generalService: GeneralService, private authService: AuthorizationService) {

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
        this.dialog.closeAll();
    }

    saveWebSiteDialog() {
        const resource = {
            url: this.getUrl.value + '',
            userId: this.authService.getUserName(),
            monitorPeriod: this.getPeriodicity.value,
            isMonitorActivated: Number(this.getActivationState.value),
        };

        this.resourceService.addResource(resource).subscribe( () => {
            this.generalService.showActionConfirmationSuccess('New resource succesfully created');
        });
        this.closeDialog();
    }
}
