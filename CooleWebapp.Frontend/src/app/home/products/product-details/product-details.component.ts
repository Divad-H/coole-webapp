import { ChangeDetectionStrategy, Component, Inject, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { BehaviorSubject, catchError, filter, of, switchMap, Subject, Subscriber, tap } from "rxjs";
import { CooleWebappApi } from "../../../../generated/coole-webapp-api";


@Component({
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductDetailsComponent implements OnInit, OnDestroy {

  public readonly states: CooleWebappApi.ProductState[] = ['Available', 'SoldOut', 'Hidden'];
  public readonly form: FormGroup;
  public readonly productId?: number;
  public readonly isNewProduct: boolean;

  public readonly submit = new Subject();
  private readonly errorResponseSubject = new BehaviorSubject('');
  readonly errorResponse = this.errorResponseSubject.asObservable();
  private readonly busySubject = new BehaviorSubject(false);
  readonly busy = this.busySubject.asObservable();
  private readonly subscriptions = new Subscriber();
    authService: any;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { product?: CooleWebappApi.IProductResponseModel },
    private readonly adminProducts: CooleWebappApi.AdminProductsClient,
    private readonly dialogRef: MatDialogRef<ProductDetailsComponent>,
    readonly fb: FormBuilder,
  ) {
    this.isNewProduct = !data.product?.id;
    this.productId = data.product?.id;
    this.form = fb.group({
      name: [data.product?.name, Validators.required],
      description: [data.product?.description],
      price: [data.product?.price, [Validators.required, Validators.min(0)]],
      state: [data.product?.state ?? 'Available', Validators.required],
    });
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  ngOnInit(): void {

    this.subscriptions.add(
      this.submit.pipe(
        tap(() => this.errorResponseSubject.next('')),
        filter(() => this.form.valid),
        tap(() => this.busySubject.next(true)),
        switchMap(() => (this.isNewProduct
          ? this.adminProducts.addProduct(
            null,
            this.form.value.name,
            this.form.value.description,
            +this.form.value.price,
            this.form.value.state)
          : this.adminProducts.editProduct(
            null,
            this.productId,
            true,
            this.form.value.name,
            this.form.value.description,
            +this.form.value.price,
            this.form.value.state)
        ).pipe(
          catchError(e => {
            if (e?.error?.error_description) {
              this.errorResponseSubject.next(e.error.error_description);
            }
            return of({ error: true });
          })
        ))
      ).subscribe(res => {
        this.busySubject.next(false);
        if ((res as any)?.error) {
          return;
        }
        this.dialogRef.close(true);
      }));
  }
}
