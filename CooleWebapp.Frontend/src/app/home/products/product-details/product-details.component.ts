import { ChangeDetectionStrategy, Component, Inject } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA } from "@angular/material/dialog";
import { CooleWebappApi } from "../../../../generated/coole-webapp-api";


@Component({
  templateUrl: './product-details.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductDetailsComponent {

  public readonly form: FormGroup;
  public readonly isNewProduct: boolean;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { product?: CooleWebappApi.IProductResponseModel },
    readonly fb: FormBuilder,
  ) {
    this.isNewProduct = !data.product?.id;
    this.form = fb.group({
      name: [data.product?.name, Validators.required],
      description: [data.product?.description],
      price: [data.product?.price, Validators.required],
      state: [data.product?.state ?? 'Available', Validators.required],
    });
  }
}
