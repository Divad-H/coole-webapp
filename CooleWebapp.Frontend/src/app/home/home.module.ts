import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule, } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { CurrencyMaskModule } from "ng2-currency-mask";
import { ImageCropperModule } from 'ngx-image-cropper';

import { HomeRoutingModule } from "./home-routing.module";
import { HomeComponent } from "./home.component";
import { SidenavService } from "./sidenav/sidenav.service";
import { ToolbarComponent } from "./toolbar/toolbar.component";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { MyPurchasesComponent } from "./my-purchases/my-purchases.component";
import { ProductsComponent } from "./products/products.component";
import { RequiresRoleDirective } from "../auth/requires-role.directive";
import { RoleGuard } from "../auth/role-guard.service";
import { CooleWebappApi } from "../../generated/coole-webapp-api";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { ConfirmDeleteComponent } from "./products/confirm-delete/confirm-delete.component";
import { ProductDetailsComponent } from "./products/product-details/product-details.component";
import { ErrorStateMatcher } from "@angular/material/core";
import { DefaultErrorStateMatcher } from "../utilities/error-state-matchers";
import { ShopComponent } from "./shop/shop.component";

@NgModule({
  declarations: [
    HomeComponent,
    ConfirmDeleteComponent,
    ToolbarComponent,
    DashboardComponent,
    MyPurchasesComponent,
    ProductsComponent,
    ProductDetailsComponent,
    RequiresRoleDirective,
    ShopComponent,
  ],
  imports: [
    HomeRoutingModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    MatSidenavModule,
    MatSortModule,
    MatTableModule,
    MatToolbarModule,
    CurrencyMaskModule,
    ImageCropperModule,
  ],
  exports: [
  ],
  providers: [
    SidenavService,
    RoleGuard,
    CooleWebappApi.AdminProductsClient,
    CooleWebappApi.ShopClient,
    { provide: ErrorStateMatcher, useClass: DefaultErrorStateMatcher },
  ]
})

export class HomeModule { }
