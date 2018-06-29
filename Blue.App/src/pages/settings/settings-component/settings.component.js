var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component } from '@angular/core';
import { Storage } from '@ionic/storage';
import { TranslateService } from 'ng2-translate';
var SettingsComponent = (function () {
    function SettingsComponent(storage, translate) {
        this.storage = storage;
        this.translate = translate;
    }
    SettingsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.storage.get('language')
            .then(function (value) {
            if (value) {
                _this.language = value;
            }
            else {
                _this.language = 'en';
            }
        });
    };
    SettingsComponent.prototype.selectLanguage = function () {
        this.storage.set('language', this.language);
        this.translate.setDefaultLang(this.language);
        this.translate.use(this.language);
    };
    return SettingsComponent;
}());
SettingsComponent = __decorate([
    Component({
        templateUrl: 'settings.html'
    }),
    __metadata("design:paramtypes", [Storage,
        TranslateService])
], SettingsComponent);
export { SettingsComponent };
//# sourceMappingURL=settings.component.js.map