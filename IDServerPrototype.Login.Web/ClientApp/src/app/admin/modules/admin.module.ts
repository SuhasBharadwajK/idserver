import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from '../admin.component';
import { DashboardComponent } from '../components/dashboard/dashboard.component';
import { NavMenuComponent } from '../components/layout/nav-menu/nav-menu.component';
import { SideBarComponent } from '../components/layout/side-bar/side-bar.component';
import { NotificationsComponent } from '../../shared/components/notifications/notifications.component';
import { ClickOutSideDirective } from '../../shared/directives/click-outside.directive';
import { ChartsModule } from '@progress/kendo-angular-charts';
import { GridModule } from '@progress/kendo-angular-grid';

@NgModule({
  declarations: [
    AdminComponent,
    NavMenuComponent,
    SideBarComponent,
    DashboardComponent,
    NotificationsComponent,
    ClickOutSideDirective],
  imports: [
    CommonModule,
    AdminRoutingModule,
    ChartsModule,
    GridModule
  ]
})
export class AdminModule { }
