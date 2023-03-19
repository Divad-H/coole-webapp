import { BreakpointObserver } from "@angular/cdk/layout";
import { ChangeDetectionStrategy, Component, Input, OnDestroy, OnInit } from "@angular/core";
import { DomSanitizer, SafeUrl } from "@angular/platform-browser";
import { catchError, EMPTY, map, Observable, of, shareReplay, Subject, Subscription, switchMap } from "rxjs";
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { CooleWebappApi } from "../../../generated/coole-webapp-api";
import { BuyDialog, DialogData } from "./buy-dialog/buy-dialog.component";
import { MatSnackBar } from "@angular/material/snack-bar";

export type Product = CooleWebappApi.IProductResponseModel & {
  image: Observable<SafeUrl | null>
}

@Component({
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-shop'
})
export class ShopComponent implements OnInit, OnDestroy {

  private readonly subscriptions = new Subscription();
  readonly products: Observable<Product[]>;
  readonly chooseProduct = new Subject<Product>();

  @Input() public productChosen: Subject<Product> | undefined;

  constructor(
    private readonly shopClient: CooleWebappApi.ShopClient,
    private readonly sanitizer: DomSanitizer,
    public readonly dialog: MatDialog,
    private readonly snackBar: MatSnackBar,
  ) {
    this.products = shopClient.getProducts(new CooleWebappApi.GetShopProductsRequestModel({
      pageIndex: 0, pageSize: 100
    })).pipe(
      map(res => res.products.map(
        p => ({
          ...p,
          image: this.shopClient.getProductImage(p.id).pipe(
            catchError(() => of(null)),
            map(fileRes => fileRes ? this.sanitizer.bypassSecurityTrustUrl(URL.createObjectURL(fileRes.data)) : null),
            shareReplay(1)
          )
        })
      )),
      shareReplay(1)
    )
  }

  ngOnInit(): void {
    this.subscriptions.add(
      this.chooseProduct.pipe(
        switchMap(product => {
          if (this.productChosen != null) {
            this.productChosen.next(product);
            return EMPTY;
          } else {
            let dialogRef: MatDialogRef<BuyDialog, any>;
            const dialogData: DialogData = {
              product,
              actions: {
                buyProducts: (data: CooleWebappApi.IBuyProductsRequestModel) =>
                  this.shopClient.buyProducts(new CooleWebappApi.BuyProductsRequestModel(data)),
                finish: (boughtProduct: string | null) => {
                  if (boughtProduct != null) {
                    this.snackBar.open(`Enjoy your ${boughtProduct}!`, 'Close', { duration: 5000 });
                  }
                  dialogRef.close();
                }
              }
            }
            dialogRef = this.dialog.open(BuyDialog, {
              data: dialogData,
            });

            return dialogRef.afterClosed();
          }
        })
      ).subscribe());
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

}
