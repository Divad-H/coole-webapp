import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from "@angular/core";
import { CooleWebappApi } from "../../../generated/coole-webapp-api";
import { Observable, shareReplay, Subscription } from "rxjs";


@Component({
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-statistics'
})
export class StatisticsComponent implements OnInit, OnDestroy {

  private readonly subscriptions = new Subscription();
  public readonly totalPurchases: Observable<CooleWebappApi.GetTotalPurchasesResponseModel>;
  public readonly topSpenders: Observable<CooleWebappApi.GetTopSpendersResponseModel[]>;

  constructor(private readonly statisticsClient: CooleWebappApi.StatisticsClient) {
    this.totalPurchases = statisticsClient.getTotalPurchases().pipe(
      shareReplay(1)
    );

    this.topSpenders = statisticsClient.getTopSpenders(
      5, "Total"
    ).pipe(
      shareReplay(1)
    );
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
