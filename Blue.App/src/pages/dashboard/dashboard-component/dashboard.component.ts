import { Component, ViewChild } from '@angular/core';
import Chart from 'chart.js';

import { ChartsComponent } from '../../charts/charts-component/charts.component';

@Component({
    selector: 'page-dashboard',
    templateUrl: 'dashboard.html'
  })
  export class DashboardComponent {
    @ViewChild('pieCanvas1') pieCanvas1;
    @ViewChild('barCanvas1') barCanvas1;
    @ViewChild('pieCanvas2') pieCanvas2;
    @ViewChild('barCanvas2') barCanvas2;

    ionViewDidLoad() {
      this.barCanvas1 = this.getBarChart(this.barCanvas1.nativeElement);
      this.pieCanvas1 = this.getPieChart(this.pieCanvas1.nativeElement);
      this.barCanvas2 = this.getBarChart(this.barCanvas2.nativeElement);
      this.pieCanvas2 = this.getPieChart(this.pieCanvas2.nativeElement);
    }

    getChart(context, chartType, data, options?) {
      return new Chart(context, {
        type: chartType,
        data: data,
        options: options
      });
    }

    getPieChart(context) {
      let data = {
        labels: ["Passed", "Repaired", "Failed"],
        datasets: [
          {
            data: [80, 15, 5],
            backgroundColor: ["#FF6384", "#36A2EB", "#FFCE56"],
            hoverBackgroundColor: ["#FF6384", "#36A2EB", "#FFCE56"]
          }]
      };
  
      return this.getChart(context, "pie", data);
    }

    getBarChart(context) {
      let data = {
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
  
      let options = {
        scales: {
          yAxes: [{
            ticks: {
              beginAtZero: true
            }
          }]
        }
      }
  
      return this.getChart(context, "bar", data, options);
    }
}