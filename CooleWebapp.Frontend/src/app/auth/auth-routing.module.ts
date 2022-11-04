import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ConfirmEmailComponent } from "./confirm-email/confirm-email.component";
import { ConfirmInitiateResetPasswordComponent } from "./confirm-initiate-reset-password/confirm-initiate-reset-password.component";
import { ConfirmRegistrationComponent } from "./confirm-registration/confirm-registration.component";
import { FinishResetPasswordComponent } from "./finish-reset-password/finish-reset-password.component";

import { LoginComponent } from "./login/login.component";
import { RegisterComponent } from "./register/register.component";
import { ResetPasswordComponent } from "./reset-password/reset-password.component";

const routes: Routes = [
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'confirm-email',
    component: ConfirmEmailComponent
  },
  {
    path: 'confirm-registration',
    component: ConfirmRegistrationComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'reset-password',
    component: ResetPasswordComponent
  },
  {
    path: 'confirm-initiate-reset-password',
    component: ConfirmInitiateResetPasswordComponent
  },
  {
    path: 'set-new-password',
    component: FinishResetPasswordComponent
  },
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class AuthRoutingModule { }
