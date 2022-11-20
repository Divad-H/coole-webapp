import { ChangeDetectionStrategy, Component, Inject } from "@angular/core";
import { MAT_DIALOG_DATA } from "@angular/material/dialog";
import { CooleWebappApi } from "../../../../generated/coole-webapp-api";


@Component({
  templateUrl: './confirm-delete.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ConfirmDeleteComponent {

  constructor(@Inject(MAT_DIALOG_DATA) public data: CooleWebappApi.IProductResponseModel) { }
}
