var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
import { Component, Inject } from '@angular/core';
import { Http, URLSearchParams } from '@angular/http';
import DataSource from 'devextreme/data/data_source';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { DataGridService } from '../data-grid.service';
var DataGridComponent = (function () {
    function DataGridComponent(http, service) {
        //Get mock data
        //this.customers = service.getCustomers();
        this.service = service;
        this.gridDataSource = {};
        this.gridDataSource = new DataSource({
            load: function (loadOptions) {
                var params = new URLSearchParams();
                params.set("skip", loadOptions.skip.toString());
                params.set("take", loadOptions.take.toString());
                params.set("sort", loadOptions.sort ? JSON.stringify(loadOptions.sort) : "");
                params.set("filter", loadOptions.filter ? JSON.stringify(loadOptions.filter) : "");
                params.set("requireTotalCount", loadOptions.requireTotalCount.toString());
                //params.set("totalSummary", loadOptions.totalSummary ? JSON.stringify(loadOptions.totalSummary : "");
                params.set("group", loadOptions.group ? JSON.stringify(loadOptions.group) : "");
                //params.set("groupSummary", loadOptions.groupSummary ? JSON.stringify(loadOptions.groupSummary) : "");
                return http.get('http://mydomain.com/MyDataService', {
                    search: params
                })
                    .toPromise()
                    .then(function (response) {
                    var json = response.json();
                    // You can process the received data here
                    return {
                        data: json.data,
                        totalCount: json.totalCount,
                        summary: json.summary
                    };
                });
            },
            insert: function (values) {
                return http.post('http://mydomain.com/MyDataService', values)
                    .toPromise();
            },
            remove: function (key) {
                return http.delete('http://mydomain.com/MyDataService' + encodeURIComponent(key))
                    .toPromise();
            },
            update: function (key, values) {
                return http.put('http://mydomain.com/MyDataService' + encodeURIComponent(key), values)
                    .toPromise();
            }
        });
    }
    return DataGridComponent;
}());
DataGridComponent = __decorate([
    Component({
        selector: 'page-data-grid',
        templateUrl: 'data-grid.html',
        providers: [DataGridService]
    }),
    __param(0, Inject(Http)),
    __metadata("design:paramtypes", [Http,
        DataGridService])
], DataGridComponent);
export { DataGridComponent };
//# sourceMappingURL=data-grid.component.js.map