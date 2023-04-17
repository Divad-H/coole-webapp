import { AfterViewInit, Component, Inject, Input, OnDestroy, OnInit, Optional, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BehaviorSubject, take, catchError, tap, map, Observable, of, startWith, Subject, Subscription, switchMap, ReplaySubject, combineLatestWith, withLatestFrom } from 'rxjs';
import { CooleWebappApi } from '../../../../generated/coole-webapp-api';
import { UserBalance } from '../../services/user-balance.service';
import { Product } from '../shop.component';

export interface IBuyActions {
  buyProducts: (products: CooleWebappApi.ProductAmount[]) => Observable<void>;
  finish: (boughtProduct: string | null) => void;
}

export interface DialogData {
  product: Product;
  actions: IBuyActions;
}

@Component({
  templateUrl: './buy-dialog.component.html',
  styleUrls: ['./buy-dialog.component.css'],
  encapsulation: ViewEncapsulation.None,
  selector: 'app-buy-dialog'
})
export class BuyDialog implements AfterViewInit, OnDestroy {

  private readonly subscriptions = new Subscription();
  readonly form: FormGroup;
  readonly total: Observable<number | string>;
  private readonly buySubject = new Subject<number>();
  public readonly busy = new BehaviorSubject(false);

  public readonly product = new ReplaySubject<Product>(1);
  private readonly actions = new ReplaySubject<IBuyActions>(1);
  @Input() public productChosen: Observable<Product> | undefined;
  @Input() public buyActions: Observable<IBuyActions> | undefined;

  constructor(
    @Optional() @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private readonly formBuilder: FormBuilder,
    private readonly snackBar: MatSnackBar,
    private readonly userBalanceService: UserBalance
  ) {
    if (data != null) {
      this.product.next(data.product);
      this.actions.next(data.actions);
    }

    this.form = formBuilder.group({
      amount: [1, [Validators.required, Validators.min(1), Validators.pattern('[0-9]*')]]
    });

    this.total = this.form.valueChanges.pipe(
      startWith({ amount: 1 }),
      switchMap(value => this.form.valid ? this.getTotal(value.amount) : of('...'))
    );
  }

  private getTotal(amount: number | string): Observable<number> {
    return this.product.pipe(map(p => +amount * p.price), take(1));
  }

  ngAfterViewInit(): void {

    if (this.productChosen != null) {
      this.subscriptions.add(
        this.productChosen.subscribe(this.product)
      );
    }
    if (this.buyActions != null) {
      this.subscriptions.add(
        this.buyActions.subscribe(this.actions)
      );
    }

    this.subscriptions.add(
      this.buySubject.pipe(
        tap(() => this.busy.next(true)),
        switchMap(amount => this.getTotal(amount).pipe(map(total => ({ total, amount })))),
        withLatestFrom(this.product),
        withLatestFrom(this.actions),
        switchMap(([[d, product], actions]) => actions.buyProducts([new CooleWebappApi.ProductAmount({
            amount: d.amount,
            expectedPrice: d.total,
            productId: product.id
          })]
        ).pipe(
          map(() => ({ productName: product.name, actions })),
          catchError(err => {
            this.snackBar.open(err?.message ?? 'An error occured.', 'Close', { duration: 5000 });
            return of(null);
          })
        ))
      ).subscribe(d => {
        this.busy.next(false);
        if (d != null) {
          this.userBalanceService.refresh();
          d.actions.finish(d.productName);
        }
      }));
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  onCloseClick(): void {
    this.actions.pipe(
      take(1)
    ).subscribe(actions => actions.finish(null))
  }

  onBuyClick(): void {
    if (this.form.valid) {
      this.buySubject.next(this.form.value.amount);
    }
  }
}
