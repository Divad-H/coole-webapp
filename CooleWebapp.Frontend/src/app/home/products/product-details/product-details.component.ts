import { ChangeDetectionStrategy, Component, Inject, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { ImageCroppedEvent } from "ngx-image-cropper";
import { BehaviorSubject, catchError, filter, of, switchMap, Subject, Subscriber, tap, empty, Observable, map, mapTo, startWith, withLatestFrom, merge } from "rxjs";
import { CooleWebappApi } from "../../../../generated/coole-webapp-api";
import { dataURItoBlob } from "../../../utilities/data-uri-to-blob"

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

  public readonly image: Observable<any | null>;
  public readonly clearImage = new Subject();

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
    readonly fb: FormBuilder
  ) {
    this.isNewProduct = data.product?.id == null;
    this.productId = data.product?.id;
    this.form = fb.group({
      name: [data.product?.name, Validators.required],
      description: [data.product?.description],
      price: [data.product?.price, [Validators.required, Validators.min(0)]],
      state: [data.product?.state ?? 'Available', Validators.required],
    });

    this.image = merge(
      this.clearImage.pipe(
        mapTo(null),
        startWith(this.productId),
        switchMap(id => id == null
          ? of(null)
          : this.adminProducts.getProductImage(id).pipe(
            catchError(e => empty()),
            map(fileResult => new File([fileResult.data], '', { type: 'image/jpeg' }))
          )),
      ),
      this.imageChanged);
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  ngOnInit(): void {

    this.subscriptions.add(
      this.clearImage.subscribe(() => this.croppedImage.next(null)));

    this.subscriptions.add(
      this.submit.pipe(
        tap(() => this.errorResponseSubject.next('')),
        filter(() => this.form.valid),
        tap(() => this.busySubject.next(true)),
        withLatestFrom(this.croppedImage.pipe(startWith(null))),
        switchMap(([_, image]) => (this.isNewProduct
          ? this.adminProducts.addProduct(
            image == null ? null : { fileName: '', data: dataURItoBlob(image) },
            this.form.value.name,
            this.form.value.description,
            +this.form.value.price,
            1.23,
            this.form.value.state)
          : this.adminProducts.editProduct(
            image == null ? null : { fileName: '', data: dataURItoBlob(image) },
            this.productId,
            false,
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

  imageChanged = new Subject<any | null>();
  croppedImage = new Subject<string | null | undefined>();

  fileChangeEvent(event: any): void {
    this.imageChanged.next(event.target.files[0]);
  }
  imageCropped(event: ImageCroppedEvent) {
    this.croppedImage.next(event.base64);
  }
  loadImageFailed() {
    // show message
  }
}
