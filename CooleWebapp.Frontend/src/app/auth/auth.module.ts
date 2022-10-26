import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";

import { MatCardModule } from "@angular/material/card"
import { ErrorStateMatcher } from "@angular/material/core";
import { MatInputModule } from "@angular/material/input"
import { MatProgressBarModule } from '@angular/material/progress-bar';

import { CooleWebappApi } from "../../generated/coole-webapp-api";
import { DefaultErrorStateMatcher } from "../utilities/error-state-matchers";
import { AuthRoutingModule } from "./auth-routing.module";
import { RegisterComponent } from "./register/register.component";

@NgModule({
  declarations: [
    RegisterComponent
  ],
  imports: [
    CommonModule,
    AuthRoutingModule,
    ReactiveFormsModule,
    MatCardModule,
    MatInputModule,
    MatProgressBarModule,
  ],
  providers: [
    { provide: ErrorStateMatcher, useClass: DefaultErrorStateMatcher },
    CooleWebappApi.RegistrationClient,
  ]
})
export class AuthModule { }
