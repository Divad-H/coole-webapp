import { StepperSelectionEvent } from "@angular/cdk/stepper";
import { AfterViewInit, ChangeDetectionStrategy, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatSnackBar } from "@angular/material/snack-bar";
import { MatStepper } from "@angular/material/stepper";
import { ActivatedRoute, Router } from "@angular/router";
import { catchError, EMPTY, map, Observable, of, shareReplay, Subject, Subscription, switchMap, throwError } from "rxjs";
import { CooleWebappApi } from "../../../generated/coole-webapp-api";
import { UserBalance } from "../services/user-balance.service";
import { IBuyActions } from "../shop/buy-dialog/buy-dialog.component";
import { Product } from "../shop/shop.component";

@Component({
  templateUrl: './shop-stepper.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styleUrls: ['shop-stepper.component.css']
})
export class ShopStepperComponent implements OnInit, AfterViewInit, OnDestroy {

  private readonly subscriptions = new Subscription();
  public readonly buyer: Observable<CooleWebappApi.IGetBuyerResponseModel>;

  public readonly productChosen = new Subject<Product>();
  public readonly products = new Subject<CooleWebappApi.ProductAmount[]>();
  public readonly buyActions: Observable<IBuyActions>;

  @ViewChild(MatStepper) stepper!: MatStepper;

  public readonly stepOneForm: FormGroup;
  public readonly stepTwoForm: FormGroup;
  public readonly stepThreeForm: FormGroup;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly fridgeClient: CooleWebappApi.FridgeClient,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly snackBar: MatSnackBar,
    private readonly userBalance: UserBalance,
  ) {

    this.stepOneForm = formBuilder.group({
      valid: [null, [Validators.required]]
    });
    this.stepTwoForm = formBuilder.group({
      valid: [null, [Validators.required]]
    });
    this.stepThreeForm = formBuilder.group({
      valid: [null, [Validators.required]]
    });

    this.buyer = this.route.queryParams.pipe(
      map(params => params['coolUserId']),
      switchMap(id => {
        if (id == null) {
          this.router.navigate(['..'], { relativeTo: this.route });
          return EMPTY;
        }
        return fridgeClient.getBuyer(+id).pipe(
          catchError(err => {
            this.router.navigate(['..'], { relativeTo: this.route });
            return EMPTY;
          })
        );
      }),
      shareReplay(1)
    );

    this.buyActions = this.buyer.pipe(
      map(buyer => {
        if (buyer.buyOnFridgePermission == "WithPinCode") {
          const res: IBuyActions = {
            buyProducts: (products: CooleWebappApi.ProductAmount[]): Observable<void> => {
              if (products[0].expectedPrice > (buyer.balance ?? 0)) {
                return throwError(() => { message: "Insufficient funds." });
              }
              this.products.next(products);
              return of(void 0);
            },
            finish: (boughtProduct: string | null) => {
              if (boughtProduct != null) {
                this.stepTwoForm.patchValue({ valid: true });
                this.stepper.next();
              } else {
                this.stepper.previous();
              }
            }
          }
          return res;
        } else {
          const res: IBuyActions = {
            buyProducts: (products: CooleWebappApi.ProductAmount[]): Observable<void> => {
              if (products[0].expectedPrice > (buyer.balance ?? 0)) {
                return throwError(() => { message: "Insufficient funds." });
              }
              return this.fridgeClient.buyProducts(
                new CooleWebappApi.BuyProductsAsFridgeRequestModel({ products, coolUserId: buyer.coolUserId }));
            },
            finish: (boughtProduct: string | null) => {
              if (boughtProduct != null) {
                this.snackBar.open(`Enjoy your ${boughtProduct}!`, 'Close', { duration: 5000 });
                this.router.navigate(['..'], { relativeTo: this.route });
              } else {
                this.stepper.previous();

              }
            }
          }
          return res;
        }
      })
    )
  }

  ngAfterViewInit(): void {
    this.subscriptions.add(
      this.productChosen.subscribe(
        () => {
          this.stepOneForm.patchValue({ valid: true })
          this.stepper.next();
        }
      ));
  }

  ngOnInit(): void {
    this.subscriptions.add(
      this.buyer.subscribe(buyer => {
        this.subscriptions.add(
          this.userBalance.setFridgeBalance(
            { balance: buyer.balance ?? 0, userName: buyer.name ?? 'unknown' },
            buyer.coolUserId ?? -1
          ));
      })
    );
  }

  onNavigate(data: StepperSelectionEvent) {
    if (data.selectedIndex == 0) {
      this.stepOneForm.patchValue({ valid: null });
    }
    if (data.selectedIndex <= 1) {
      this.stepTwoForm.patchValue({ valid: null });
    }
  }

  backClicked() {
    this.router.navigate(['..'], { relativeTo: this.route });
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
