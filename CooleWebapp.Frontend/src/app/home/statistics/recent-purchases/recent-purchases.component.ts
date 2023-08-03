import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from "@angular/core";
import { Observable, shareReplay, Subscription } from "rxjs";
import { CooleWebappApi } from "../../../../generated/coole-webapp-api";

@Component({
  selector: 'app-recent-purchases',
  templateUrl: './recent-purchases.component.html',
  styleUrls: ['../statistics.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RecentPurchasesComponent implements OnInit, OnDestroy {

  private readonly subscriptions = new Subscription();
  public readonly recentPurchases: Observable<CooleWebappApi.GetMostRecentPurchasesResponseModel[]>;

  displayedColumns: string[] = ['initials', 'product', 'quantity', 'price'];

  constructor(private readonly statisticsClient: CooleWebappApi.StatisticsClient) {

    this.recentPurchases = statisticsClient.getMostRecentPurchases(10).pipe(
      shareReplay(1)
    );
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
