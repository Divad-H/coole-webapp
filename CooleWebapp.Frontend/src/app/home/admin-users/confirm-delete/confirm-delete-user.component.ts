import { ChangeDetectionStrategy, Component, Inject } from "@angular/core";
import { MAT_DIALOG_DATA } from "@angular/material/dialog";
import { CooleWebappApi } from "../../../../generated/coole-webapp-api";


@Component({
  templateUrl: './confirm-delete-user.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-confirm-delete-user'
})
export class ConfirmDeleteUserComponent {

  constructor(@Inject(MAT_DIALOG_DATA) public data: CooleWebappApi.IUserResponseModel) { }
}
