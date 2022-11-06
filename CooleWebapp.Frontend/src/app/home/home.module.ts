import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule, } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';

import { HomeRoutingModule } from "./home-routing.module";
import { HomeComponent } from "./home.component";
import { SidenavService } from "./sidenav/sidenav.service";
import { ToolbarComponent } from "./toolbar/toolbar.component";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { MyPurchasesComponent } from "./my-purchases/my-purchases.component";
import { ProductsComponent } from "./products/products.component";
import { RequiresRoleDirective } from "../auth/requires-role.directive";
import { RoleGuard } from "../auth/role-guard.service";

@NgModule({
  declarations: [
    HomeComponent,
    ToolbarComponent,
    DashboardComponent,
    MyPurchasesComponent,
    ProductsComponent,
    RequiresRoleDirective,
  ],
  imports: [
    HomeRoutingModule,
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatListModule,
    MatSidenavModule,
    MatToolbarModule,
  ],
  exports: [
  ],
  providers: [
    SidenavService,
    RoleGuard,
  ]
})

export class HomeModule { }
