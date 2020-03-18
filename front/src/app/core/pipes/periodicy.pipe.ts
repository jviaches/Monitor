import { Pipe, PipeTransform } from '@angular/core';
import { Periodicy } from '../enums/periodicy';

@Pipe({name: 'periodicy'})
export class PeriodicyPipe implements PipeTransform {
  transform(periodicy: string): string {

    const num = Number(periodicy);
    switch (num) {
        case Periodicy.OnceInHour:
            return 'Every Hour';
        case 2: // TODO: Periodicy.OnceInDay
            return 'Once a Day';
        case 3: // TODO: Periodicy.OnceInWeek
            return 'Once a Week';
        case 4: // TODO: Periodicy.OnceInMonth
            return 'Once a Month';
        default:
            return '-';
    }
  }
}
