import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from "@angular/core";
import { CooleWebappApi } from "../../../generated/coole-webapp-api";
import { filter, map, Observable, shareReplay, Subject, Subscription, withLatestFrom } from "rxjs";
import { ActivatedRoute, Router } from "@angular/router";
import { AuthService } from "../../auth/auth.service";

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DashboardComponent implements OnInit, OnDestroy {

  private readonly subscriptions = new Subscription();
  private readonly onCardClickSubject = new Subject<CooleWebappApi.IBuyerResponseModel>();

  public recentBuyers: Observable<CooleWebappApi.IGetRecentBuyersResponeModel>;
  public isFridge: Observable<boolean>;

  constructor(
    private readonly dashboardClient: CooleWebappApi.DashboardClient,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly authService: AuthService,
  ) {
    this.recentBuyers = dashboardClient.getRecentBuyers(0, 100).pipe(
      shareReplay(1)
    );

    this.isFridge = authService.roles.pipe(
      map(roles => roles.includes('Fridge')),
      shareReplay(1)
    );
  }

  ngOnInit(): void {
    this.subscriptions.add(
      this.onCardClickSubject.pipe(
        withLatestFrom(this.isFridge),
        filter(([_, isFridge]) => isFridge)
      ).subscribe(([buyer, _]) => {
        this.router.navigate(['shop'], { relativeTo: this.route, queryParams: { coolUserId: buyer.coolUserId } });
      })
    );
  }

  onCardClick(buyer: CooleWebappApi.IBuyerResponseModel) {
    if (!buyer.canBuyOnFridge) {
      return;
    }
    this.onCardClickSubject.next(buyer)
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
