import { Component, Inject } from '@angular/core';
import { Http, HttpModule, URLSearchParams } from '@angular/http';
import { DxDataGridModule, DxButtonModule  } from 'devextreme-angular';
import DataSource from 'devextreme/data/data_source';
import CustomStore from 'devextreme/data/custom_store';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';

import { Customer, DataGridService } from '../data-grid.service';

@Component({
  selector: 'page-data-grid',
  templateUrl: 'data-grid.html',
  providers: [DataGridService]
})

export class DataGridComponent {
  customers: Customer[];
  gridDataSource: any = {};

  constructor(
    @Inject(Http) http: Http,
    private service: DataGridService) {
    //Get mock data
    this.gridDataSource = service.getCustomers();

    // this.gridDataSource = new DataSource({
    //   load: function (loadOptions) {
    //       let params: URLSearchParams = new URLSearchParams();
    //       params.set("skip", loadOptions.skip.toString());
    //       params.set("take", loadOptions.take.toString());
    //       params.set("sort", loadOptions.sort ? JSON.stringify(loadOptions.sort) : "");
    //       params.set("filter", loadOptions.filter ? JSON.stringify(loadOptions.filter) : "");
    //       params.set("requireTotalCount", loadOptions.requireTotalCount.toString());
    //       //params.set("totalSummary", loadOptions.totalSummary ? JSON.stringify(loadOptions.totalSummary : "");
    //       params.set("group", loadOptions.group ? JSON.stringify(loadOptions.group) : "");
    //       //params.set("groupSummary", loadOptions.groupSummary ? JSON.stringify(loadOptions.groupSummary) : "");
    //       return http.get('http://mydomain.com/MyDataService', {
    //                       search: params
    //                   })
    //                   .toPromise()
    //                   .then(response => {
    //                       var json = response.json();
    //                       // You can process the received data here
    //                       return {
    //                           data: json.data,
    //                           totalCount: json.totalCount,
    //                           summary: json.summary
    //                       }
    //                   });
    //                 },
                    
    //   insert: function (values) {
    //       return http.post('http://mydomain.com/MyDataService', values)
    //                   .toPromise();
    //   },
  
    //   remove: function (key) {
    //       return http.delete('http://mydomain.com/MyDataService' + encodeURIComponent(key))
    //                   .toPromise();
    //   },

    //   update: function (key, values) {
    //       return http.put('http://mydomain.com/MyDataService' + encodeURIComponent(key), values)
    //                   .toPromise();
    //   }
    // });
  }

  customizeColumns(columns){
    columns[0].width = 20;
    columns[1].width = 70;
    columns[2].width = 70;
    columns[3].width = 70;
    columns[4].width = 70;
  }
}