import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule, } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatListModule } from '@angular/material/list';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
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
import { BuyDialog } from "./shop/buy-dialog/buy-dialog.component";
import { UserBalance } from "./services/user-balance.service";
import { PayDialogComponent } from "./pay-dialog/pay-dialog.component";
import { SettingsDialogComponent } from "./settings-dialog/settings-dialog.component";
import { ShopStepperComponent } from "./shop-stepper/shop-stepper.component";
import { PinPadComponent } from "./shop-stepper/pin-pad/pin-pad.component";
import { StatisticsComponent } from "./statistics/statistics.component";
import { NgChartsModule } from "ng2-charts";
import { TopSpendersComponent } from "./statistics/top-spenders/top-spenders.component";
import { PurchasesChartComponent } from "./statistics/purchases-chart/purchases-chart.component";
import { ProductsChartComponent } from "./statistics/products-chart/products-chart.component";

@NgModule({
  declarations: [
    BuyDialog,
    HomeComponent,
    ConfirmDeleteComponent,
    ToolbarComponent,
    DashboardComponent,
    MyPurchasesComponent,
    ProductsComponent,
    ProductDetailsComponent,
    RequiresRoleDirective,
    ShopComponent,
    ShopStepperComponent,
    PayDialogComponent,
    SettingsDialogComponent,
    StatisticsComponent,
    PinPadComponent,
    ProductsChartComponent,
    PurchasesChartComponent,
    TopSpendersComponent,
  ],
  imports: [
    HomeRoutingModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatCheckboxModule,
    MatDialogModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    MatSidenavModule,
    MatSnackBarModule,
    MatSortModule,
    MatStepperModule,
    MatTableModule,
    MatTooltipModule,
    MatToolbarModule,
    CurrencyMaskModule,
    ImageCropperModule,
    NgChartsModule,
  ],
  exports: [
  ],
  providers: [
    SidenavService,
    RoleGuard,
    UserBalance,
    CooleWebappApi.AdminProductsClient,
    CooleWebappApi.ShopClient,
    CooleWebappApi.UserAccountClient,
    CooleWebappApi.DashboardClient,
    CooleWebappApi.UserSettingsClient,
    CooleWebappApi.FridgeClient,
    CooleWebappApi.StatisticsClient,
    { provide: ErrorStateMatcher, useClass: DefaultErrorStateMatcher },
  ]
})

export class HomeModule { }
