import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ConfirmEmailComponent } from "./confirm-email/confirm-email.component";
import { ConfirmRegistrationComponent } from "./confirm-registration/confirm-registration.component";

import { LoginComponent } from "./login/login.component";
import { RegisterComponent } from "./register/register.component";

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
