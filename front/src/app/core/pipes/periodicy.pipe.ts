import { Pipe, PipeTransform } from '@angular/core';
import { SelectionOptions } from '../shared/selection-options';

@Pipe({name: 'periodicy'})
export class PeriodicyPipe implements PipeTransform {
  transform(periodicy: number): string {

    const periodicity = SelectionOptions.periodicityOptions().find( item => item.key === periodicy);
    if (periodicity) {
        return periodicity.value;
    }
    return '-';
  }
}
