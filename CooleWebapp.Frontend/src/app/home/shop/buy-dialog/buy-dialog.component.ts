import { Component, Inject, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BehaviorSubject, catchError, tap, map, mapTo, Observable, of, startWith, Subject, Subscription, switchMap } from 'rxjs';
import { CooleWebappApi } from '../../../../generated/coole-webapp-api';
import { Product } from '../shop.component';

export interface DialogData {
  product: Product;
}

@Component({
  templateUrl: './buy-dialog.component.html',
  styleUrls: ['./buy-dialog.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class BuyDialog implements OnInit, OnDestroy {

  private readonly subscriptions = new Subscription();
  readonly form: FormGroup;
  readonly total: Observable<number | string>;
  private readonly buySubject = new Subject();
  public readonly busy = new BehaviorSubject(false);

  constructor(
    public readonly dialogRef: MatDialogRef<BuyDialog>,
    @Inject(MAT_DIALOG_DATA) public readonly data: DialogData,
    private readonly formBuilder: FormBuilder,
    private readonly shopClient: CooleWebappApi.ShopClient,
  ) {
    this.form = formBuilder.group({
      amount: [1, [Validators.required, Validators.min(1), Validators.pattern('[0-9]*')]]
    });
    this.total = this.form.valueChanges.pipe(
      startWith({ amount: 1 }),
      map(v => this.form.valid ? v.amount * data.product.price : '...')
    );
  }

  ngOnInit(): void {
    this.subscriptions.add(
      this.buySubject.pipe(
        tap(() => this.busy.next(true)),
        switchMap(() => 
          this.shopClient.buyProducts(new CooleWebappApi.BuyProductsRequestModel({
            products: [ new CooleWebappApi.ProductAmount({
              amount: this.form.value.amount,
              expectedPrice: this.form.value.amount * this.data.product.price,
              productId: this.data.product.id
            })]
          })).pipe(
            mapTo(true),
            catchError(err => { console.log(err); return of(false) })
          ))
      ).subscribe(success => {
        this.busy.next(false);
        if (success) {
          this.dialogRef.close(this.form.value.amount);
        }
      })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onBuyClick(): void {
    if (this.form.valid) {
      this.buySubject.next({});
    }
  }
}
