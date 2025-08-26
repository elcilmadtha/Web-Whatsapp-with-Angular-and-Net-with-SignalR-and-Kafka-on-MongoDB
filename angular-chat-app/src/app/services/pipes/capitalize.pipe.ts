import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'capitalize'
})
export class CapitalizePipe implements PipeTransform {

  transform(value: string): string {
    if (!value) return '';

    return value
      .toLowerCase() // make all lowercase first
      .split(' ')    // split by spaces
      .map(word => word.charAt(0).toUpperCase() + word.slice(1))
      .join(' ');
  }
}
