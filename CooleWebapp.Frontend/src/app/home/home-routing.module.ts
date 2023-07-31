import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { RoleGuard } from "../auth/role-guard.service";
import { DashboardComponent } from "./dashboard/dashboard.component";

import { HomeComponent } from "./home.component";
import { MyPurchasesComponent } from "./my-purchases/my-purchases.component";
import { ProductsComponent } from "./products/products.component";
import { ShopStepperComponent } from "./shop-stepper/shop-stepper.component";
import { ShopComponent } from "./shop/shop.component";
import { StatisticsComponent } from "./statistics/statistics.component";

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'dashboard'
  },
  {
    path: '',
    component: HomeComponent,
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
      },
      {
        path: 'dashboard/shop',
        component: ShopStepperComponent,
        canActivate: [RoleGuard],
        data: {
          roles: ['Fridge']
        }
      },
      {
        path: 'my-purchases',
        component: MyPurchasesComponent,
        canActivate: [RoleGuard],
        data: {
          roles: ['User']
        }
      },
      {
        path: 'shop',
        component: ShopComponent,
        canActivate: [RoleGuard],
        data: {
          roles: ['User']
        }
      },
      {
        path: 'products',
        component: ProductsComponent,
        canActivate: [RoleGuard],
        data: {
          roles: ['Administrator']
        }
      },
      {
        path: 'statistics',
        component: StatisticsComponent,
      },
    ]
  },
];


@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule,
  ]
})
export class HomeRoutingModule { }
