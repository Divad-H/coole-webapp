import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";

import { MatCardModule } from "@angular/material/card"
import { ErrorStateMatcher } from "@angular/material/core";
import { MatInputModule } from "@angular/material/input"
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatButtonModule } from '@angular/material/button';

import { CooleWebappApi } from "../../generated/coole-webapp-api";
import { DefaultErrorStateMatcher } from "../utilities/error-state-matchers";
import { AuthRoutingModule } from "./auth-routing.module";
import { LoginComponent } from "./login/login.component";
import { RegisterComponent } from "./register/register.component";
import { ConfirmRegistrationComponent } from "./confirm-registration/confirm-registration.component";
import { ConfirmEmailComponent } from "./confirm-email/confirm-email.component";
import { ResetPasswordComponent } from "./reset-password/reset-password.component";
import { FinishResetPasswordComponent } from "./finish-reset-password/finish-reset-password.component";
import { ConfirmInitiateResetPasswordComponent } from "./confirm-initiate-reset-password/confirm-initiate-reset-password.component";

@NgModule({
  declarations: [
    RegisterComponent,
    ConfirmRegistrationComponent,
    LoginComponent,
    ConfirmEmailComponent,
    ResetPasswordComponent,
    FinishResetPasswordComponent,
    ConfirmInitiateResetPasswordComponent,
  ],
  imports: [
    CommonModule,
    AuthRoutingModule,
    ReactiveFormsModule,
    MatCardModule,
    MatInputModule,
    MatProgressBarModule,
    MatButtonModule,
  ],
  providers: [
    { provide: ErrorStateMatcher, useClass: DefaultErrorStateMatcher },
    CooleWebappApi.RegistrationClient,
  ]
})
export class AuthModule { }
