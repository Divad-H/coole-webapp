import { ChangeDetectionStrategy, Component } from "@angular/core";
import { CooleWebappApi } from "../../../generated/coole-webapp-api";
import { Observable, shareReplay } from "rxjs";

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DashboardComponent {

  public recentBuyers: Observable<CooleWebappApi.IGetRecentBuyersResponeModel>;

  constructor(
    private readonly dashboardClient: CooleWebappApi.DashboardClient,
  ) {
    this.recentBuyers = dashboardClient.getRecentBuyers(0, 100).pipe(
      shareReplay(1)
    );
  }
}
