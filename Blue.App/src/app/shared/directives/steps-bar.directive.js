var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Directive } from '@angular/core';
var StepsBar = (function () {
    function StepsBar() {
    }
    return StepsBar;
}());
StepsBar = __decorate([
    Directive({
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
], StepsBar);
export { StepsBar };
//# sourceMappingURL=steps-bar.directive.js.map