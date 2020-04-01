import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogData } from 'src/app/core/components/modal-dialog/dialog-data';
import { ResourceService } from 'src/app/core/services/resource.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SelectionOptions, SelectionOption } from 'src/app/core/shared/selection-options';
import { GeneralService } from 'src/app/core/services/general.service';
import { IResource } from 'src/app/core/models/resource.model';

@Component({
    selector: 'app-root',
    templateUrl: './resource-edit.component.html',
    styleUrls: ['./resource-edit.component.scss']
})
export class ResourceEditComponent implements OnInit {

    resourceToEdit: IResource;
    selectedPeriodicity: SelectionOption;

    siteFormGroup: FormGroup;
    periodicityOptions = SelectionOptions.periodicityOptions();
    urlRegex = '^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$';

    constructor(public dialog: MatDialog, @Inject(MAT_DIALOG_DATA) { data }: DialogData,
                private resourceService: ResourceService, private formBuilder: FormBuilder,
                private generalService: GeneralService) {

        this.resourceToEdit = data;
    }


    ngOnInit(): void {
        this.selectedPeriodicity = this.periodicityOptions.find( item => item.key === this.resourceToEdit.monitorPeriod / 1000);
        // console.log(this.selectedPeriodicity);

        this.siteFormGroup = this.formBuilder.group({
            url: [this.resourceToEdit.url, [Validators.required, Validators.pattern(this.urlRegex)]],
            periodicity: [this.selectedPeriodicity, Validators.required],
            isActivated: [this.resourceToEdit.isMonitorActivated]
        });

        console.log(this.siteFormGroup.get('periodicity').value);
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

    comparePeriodicity(object1: SelectionOption, object2: SelectionOption) {
        return object1 && object2 && object1.key === object2.key;
    }

    closeDialog() {
        console.log(this.getActivationState.value);
        this.dialog.closeAll();
    }

    UpdateDialog() {

        const timeOptions = {
            year: 'numeric', month: 'numeric', day: 'numeric',
            hour: 'numeric', minute: 'numeric', second: 'numeric',
            hour12: false
          };

        const resource = {
            id: this.resourceToEdit.id,
            url: this.getUrl.value + '',
            userId: '1',
            monitorPeriod: this.getPeriodicity.value,
            isMonitorActivated: this.getActivationState.value === true ? '1' : '0',
            monitorActivationDate: new Intl.DateTimeFormat('en-US', timeOptions).format(new Date()).toString()
        };

        this.resourceService.updateResource(resource).subscribe( () => {
            this.generalService.showActionConfirmation(`Resource ${resource.url} succesfully updated`);
        });
        this.closeDialog();
    }
}