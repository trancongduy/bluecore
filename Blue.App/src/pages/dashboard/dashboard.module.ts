import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../app/shared/shared.module';
import { DashboardComponent } from './dashboard-component/dashboard.component';

@NgModule({
    declarations: [
        DashboardComponent
    ],
    imports: [
      CommonModule,
      SharedModule
    ],
    exports: [
        DashboardComponent
    ],
    entryComponents: [
        DashboardComponent
    ]
  })
  export class DashboardModule { }