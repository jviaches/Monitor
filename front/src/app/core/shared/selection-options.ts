import { Injectable } from '@angular/core';

export interface SelectionOption {
    key: any;
    value: any;
}

@Injectable()
export class SelectionOptions {
    static periodicityOptions(): SelectionOption[] {
        return [
            { key: 1, value: '1 min'},
            { key: 5, value: '5 min'},
            { key: 60, value: '1h'},
            { key: 10080, value: '1w'},
            { key: 43800, value: '1month'},
        ];
    }
}
