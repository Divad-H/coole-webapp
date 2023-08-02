import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from "@angular/core";
import { BehaviorSubject, combineLatest, map, Observable, shareReplay, Subscription, switchMap } from "rxjs";
import { ChartConfiguration } from 'chart.js';
import { CooleWebappApi } from "../../../../generated/coole-webapp-api";
import { MatSelectChange } from "@angular/material/select";

const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

function makeMonthLabels(year: number, month: number, count: number): string[] {
  const res: string[] = [];
  for (let i = 0; i < count; ++i) {
    res.push(`${months[month - 1]} ${year}`);
    if (++month > 12) {
      month = 1;
      ++year;
    }
  }
  return res;
}

@Component({
  selector: 'app-purchases-chart',
  templateUrl: './purchases-chart.component.html',
  styleUrls: ['../statistics.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PurchasesChartComponent implements OnInit, OnDestroy {

  private readonly subscriptions = new Subscription();
  public readonly purchasesTimePeriod = new BehaviorSubject<CooleWebappApi.PurchaseStatisticsTimePeriod>("OneYear");
  public readonly purchasesChartData: Observable<ChartConfiguration<'line'>['data']>;
  public readonly products: Observable<CooleWebappApi.ShortProductResponseModel[]>;
  public readonly productFilter = new BehaviorSubject<number>(0);
  
  public purchasesChartOptions: ChartConfiguration<'line'>['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    
    scales: {
      x: {
        grid: {
          display: false
        },
      },
      y: {
        beginAtZero: true,
        ticks: {
          stepSize: 1
        },
        grid: {
          display: false
        },
      },
    },
  };

  constructor(
    private readonly statisticsClient: CooleWebappApi.StatisticsClient,
    private readonly shopClient: CooleWebappApi.ShopClient
  ) {

    this.purchasesChartData = combineLatest(
      [this.purchasesTimePeriod, this.productFilter],
      (purchasesTimePeriod, productFilter) => ({purchasesTimePeriod, productFilter })).pipe(
      switchMap(config => statisticsClient.getPurchasesPerTimeStatistics(config.productFilter || null, config.purchasesTimePeriod)),
      map(res => ({
        labels: makeMonthLabels(res.startYear, res.startMonth, res.numberOfPurchases.length),
        datasets: [
          { data: res.numberOfPurchases, fill: true }
        ]
      })),
      shareReplay(1)
    );

    this.products = this.shopClient.getShortProducts().pipe(
      shareReplay(1)
    );
  }

  public purchasesTimePeriodChanged(value: MatSelectChange) {
    this.purchasesTimePeriod.next(value.value);
  }

  public productFilterChanged(value: MatSelectChange) {
    this.productFilter.next(value.value);
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
