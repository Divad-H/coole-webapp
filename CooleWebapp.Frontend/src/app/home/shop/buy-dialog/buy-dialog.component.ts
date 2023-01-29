import { Component, Inject, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { map, Observable, startWith } from 'rxjs';
import { Product } from '../shop.component';

export interface DialogData {
  product: Product;
}

@Component({
  templateUrl: './buy-dialog.component.html',
  styleUrls: ['./buy-dialog.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class BuyDialog {

  readonly form: FormGroup;
  readonly total: Observable<number | string>;

  constructor(
    public readonly dialogRef: MatDialogRef<BuyDialog>,
    @Inject(MAT_DIALOG_DATA) public readonly data: DialogData,
    private readonly formBuilder: FormBuilder
  ) {
    this.form = formBuilder.group({
      amount: [1, [Validators.required, Validators.min(1), Validators.pattern('[0-9]*')]]
    });
    this.total = this.form.valueChanges.pipe(
      startWith({ amount: 1 }),
      map(v => this.form.valid ? v.amount * data.product.price : '...')
    );
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onBuyClick(): void {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value.amount);
    }
  }
}
