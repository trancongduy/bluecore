import { Directive } from '@angular/core';

@Directive({
  selector: 'steps-bar',
  inputs: [
    'numOfSteps',
    'numsColor',
    'componentBack',
    'barHeigth',
    'componentMargin',
    'highColor',
    'backColor',
    'radius',
    'dir',
    'testing',
    'areola'
  ]
})
export class StepsBar {

}