import { Pipe, PipeTransform } from '@angular/core';
import { Periodicy } from '../enums/periodicy';

@Pipe({name: 'periodicy'})
export class PeriodicyPipe implements PipeTransform {
  transform(periodicy: string): string {

    const num = Number(periodicy);
    switch (num) {
        case Periodicy.OnceInHour:
            return 'Houry';
        case 2: // TODO: Periodicy.OnceInDay
            return 'Daily';
        case 3: // TODO: Periodicy.OnceInWeek
            return 'Weekly';
        case 4: // TODO: Periodicy.OnceInMonth
            return 'Monthly';
        default:
            return '-';
    }
  }
}
