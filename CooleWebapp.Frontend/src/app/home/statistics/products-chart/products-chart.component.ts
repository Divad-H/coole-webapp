import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from "@angular/core";
import { BehaviorSubject, map, Observable, shareReplay, Subscription, switchMap } from "rxjs";
import { ChartConfiguration } from 'chart.js';
import { CooleWebappApi } from "../../../../generated/coole-webapp-api";
import { MatSelectChange } from "@angular/material/select";

@Component({
  selector: 'app-products-chart',
  templateUrl: './products-chart.component.html',
  styleUrls: ['../statistics.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProductsChartComponent implements OnInit, OnDestroy {

  private readonly subscriptions = new Subscription();
  public readonly productsTimePeriod = new BehaviorSubject<CooleWebappApi.TimePeriod>("Total");
  public readonly productsChartData: Observable<ChartConfiguration<'doughnut'>['data']>;

  public productsChartOptions: ChartConfiguration<'doughnut'>['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    
    plugins: {
      legend: {
        position: 'bottom',
      },
    }
  };

  constructor(
    private readonly statisticsClient: CooleWebappApi.StatisticsClient,
    private readonly shopClient: CooleWebappApi.ShopClient
  ) {

    this.productsChartData = this.productsTimePeriod.pipe(
      switchMap(timePeriod => statisticsClient.getProductStatistics(timePeriod)),
      map(res => ({
        labels: res.map(p => p.productName),
        datasets: [
          { data: res.map(p => p.numberOfPurchases), label: 'Number of Purchases' }
        ]
      })),
      shareReplay(1)
    );
  }

  public productsTimePeriodChanged(value: MatSelectChange) {
    this.productsTimePeriod.next(value.value);
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
