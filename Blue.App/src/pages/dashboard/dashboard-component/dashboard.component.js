var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ViewChild } from '@angular/core';
import Chart from 'chart.js';
var DashboardComponent = (function () {
    function DashboardComponent() {
    }
    DashboardComponent.prototype.ionViewDidLoad = function () {
        this.barCanvas1 = this.getBarChart(this.barCanvas1.nativeElement);
        this.pieCanvas1 = this.getPieChart(this.pieCanvas1.nativeElement);
        this.barCanvas2 = this.getBarChart(this.barCanvas2.nativeElement);
        this.pieCanvas2 = this.getPieChart(this.pieCanvas2.nativeElement);
    };
    DashboardComponent.prototype.getChart = function (context, chartType, data, options) {
        return new Chart(context, {
            type: chartType,
            data: data,
            options: options
        });
    };
    DashboardComponent.prototype.getPieChart = function (context) {
        var data = {
            labels: ["Passed", "Repaired", "Failed"],
            datasets: [
                {
                    data: [80, 15, 5],
                    backgroundColor: ["#FF6384", "#36A2EB", "#FFCE56"],
                    hoverBackgroundColor: ["#FF6384", "#36A2EB", "#FFCE56"]
                }
            ]
        };
        return this.getChart(context, "pie", data);
    };
    DashboardComponent.prototype.getBarChart = function (context) {
        var data = {
            labels: ["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
            datasets: [{
                    label: '# of Votes',
                    data: [12, 19, 3, 5, 2, 3],
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(255, 206, 86, 0.2)',
                        'rgba(75, 192, 192, 0.2)',
                        'rgba(153, 102, 255, 0.2)',
                        'rgba(255, 159, 64, 0.2)'
                    ],
                    borderColor: [
                        'rgba(255,99,132,1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)',
                        'rgba(153, 102, 255, 1)',
                        'rgba(255, 159, 64, 1)'
                    ],
                    borderWidth: 1
                }]
        };
        var options = {
            scales: {
                yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
            }
        };
        return this.getChart(context, "bar", data, options);
    };
    return DashboardComponent;
}());
__decorate([
    ViewChild('pieCanvas1'),
    __metadata("design:type", Object)
], DashboardComponent.prototype, "pieCanvas1", void 0);
__decorate([
    ViewChild('barCanvas1'),
    __metadata("design:type", Object)
], DashboardComponent.prototype, "barCanvas1", void 0);
__decorate([
    ViewChild('pieCanvas2'),
    __metadata("design:type", Object)
], DashboardComponent.prototype, "pieCanvas2", void 0);
__decorate([
    ViewChild('barCanvas2'),
    __metadata("design:type", Object)
], DashboardComponent.prototype, "barCanvas2", void 0);
DashboardComponent = __decorate([
    Component({
        selector: 'page-dashboard',
        templateUrl: 'dashboard.html'
    })
], DashboardComponent);
export { DashboardComponent };
//# sourceMappingURL=dashboard.component.js.map