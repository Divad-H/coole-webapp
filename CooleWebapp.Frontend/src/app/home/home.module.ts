import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule, } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';

import { HomeRoutingModule } from "./home-routing.module";
import { HomeComponent } from "./home.component";
import { SidenavService } from "./sidenav/sidenav.service";
import { ToolbarComponent } from "./toolbar/toolbar.component";

@NgModule({
  declarations: [
    HomeComponent,
    ToolbarComponent,
  ],
  imports: [
    HomeRoutingModule,
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatSidenavModule,
    MatToolbarModule,
  ],
  exports: [
  ],
  providers: [
    SidenavService,
  ]
})

export class HomeModule { }
