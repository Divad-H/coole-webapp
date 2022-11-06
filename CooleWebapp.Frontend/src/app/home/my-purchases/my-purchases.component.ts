import { ChangeDetectionStrategy, Component } from "@angular/core";

@Component({
  templateUrl: './my-purchases.component.html',
  styleUrls: ['./my-purchases.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MyPurchasesComponent { }
