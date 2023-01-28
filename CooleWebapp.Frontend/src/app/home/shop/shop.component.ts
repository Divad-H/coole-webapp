import { BreakpointObserver } from "@angular/cdk/layout";
import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from "@angular/core";
import { DomSanitizer, SafeUrl } from "@angular/platform-browser";
import { catchError, map, Observable, of, shareReplay, Subscription } from "rxjs";
import { CooleWebappApi } from "../../../generated/coole-webapp-api";

type Product = CooleWebappApi.IProductResponseModel & {
  image: Observable<SafeUrl | null>
}

@Component({
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ShopComponent implements OnInit, OnDestroy {

  private readonly subscriptions = new Subscription();
  readonly products: Observable<Product[]>;

  constructor(
    private readonly breakpointObserver: BreakpointObserver,
    private readonly shopClient: CooleWebappApi.ShopClient,
    private readonly sanitizer: DomSanitizer,
  ) {
    this.products = shopClient.getProducts(new CooleWebappApi.GetShopProductsRequestModel({
      pageIndex: 0, pageSize: 100
    })).pipe(
      map(res => res.products.map(
        p => ({
          ...p,
          image: this.shopClient.getProductImage(p.id).pipe(
            catchError(() => of(null)),
            map(fileRes => fileRes ? this.sanitizer.bypassSecurityTrustUrl(URL.createObjectURL(fileRes.data)) : null)
          )
        })
      )),
      shareReplay(1)
    )
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

}
