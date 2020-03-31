import { Injectable } from '@angular/core';

export interface SelectionOption {
    key: any;
    value: any;
}

@Injectable()
export class SelectionOptions {
    static periodicityOptions(): SelectionOption[] {
        return [
            { key: '5', value: 'Every 5 minutes'},
            { key: '60', value: 'Hourly'},
            { key: '10080', value: 'Weekly'},
            { key: '43800', value: 'Monthly'},
        ];
    }
}
