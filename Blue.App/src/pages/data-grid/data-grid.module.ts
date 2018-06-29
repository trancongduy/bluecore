import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../app/shared/shared.module';
import { DataGridComponent } from "./data-grid-component/data-grid.component";

import { DxDataGridModule, DxButtonModule } from 'devextreme-angular';

@NgModule({
  declarations: [
    DataGridComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    DxDataGridModule,
    DxButtonModule
  ],
  exports: [
    DataGridComponent
  ],
  entryComponents: [
    DataGridComponent
  ]
})
export class DataGridModule { }
