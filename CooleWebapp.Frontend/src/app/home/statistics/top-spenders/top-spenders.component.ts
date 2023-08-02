import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from "@angular/core";
import { BehaviorSubject, map, Observable, shareReplay, Subscription, switchMap } from "rxjs";
import { ChartConfiguration } from 'chart.js';
import { CooleWebappApi } from "../../../../generated/coole-webapp-api";
import { MatSelectChange } from "@angular/material/select";

@Component({
  selector: 'app-top-spenders',
  templateUrl: './top-spenders.component.html',
  styleUrls: ['../statistics.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TopSpendersComponent implements OnInit, OnDestroy {

  private readonly subscriptions = new Subscription();
  public readonly spendersTimePeriod = new BehaviorSubject<CooleWebappApi.TimePeriod>("Total");
  public readonly spendersChartData: Observable<ChartConfiguration<'bar'>['data']>;

  public spendersChartOptions: ChartConfiguration<'bar'>['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
      x: {
        grid: {
          display: false
        },
      },
      y: {
        grid: {
          display: false
        },
        ticks: {
          callback: (value) => `${(value as number).toFixed(2)} €`
        }
      },
    },
    plugins: {
      tooltip: {
        callbacks: {
          label: (data) => ` ${(data.dataset.data[data.dataIndex] as number).toFixed(2)} €`
        }
      },
    }
  };

  constructor(private readonly statisticsClient: CooleWebappApi.StatisticsClient) {

    this.spendersChartData = this.spendersTimePeriod.pipe(
      switchMap(timePeriod => statisticsClient.getTopSpenders(5, timePeriod)),
      map(res => ({
        labels: res.map(r => r.initials),
        datasets: [
          { data: res.map(r => r.amountSpent), maxBarThickness: 128 }
        ]
      })),
      shareReplay(1)
    );
  }

  public spendersTimePeriodChanged(value: MatSelectChange) {
    this.spendersTimePeriod.next(value.value);
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
