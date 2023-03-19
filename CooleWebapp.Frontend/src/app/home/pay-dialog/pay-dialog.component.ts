import { ChangeDetectionStrategy, Component, OnDestroy, OnInit, Optional } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";
import { BehaviorSubject, Subject, Subscriber, switchMap, tap } from "rxjs";
import { UserBalance } from "../services/user-balance.service";


@Component({
  templateUrl: './pay-dialog.component.html',
  styleUrls: ['./pay-dialog.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-pay-dialog'
})
export class PayDialogComponent implements OnInit, OnDestroy {

  public readonly busy = new BehaviorSubject(false);
  public readonly submitted = new BehaviorSubject(false);
  private readonly subscriptions = new Subscriber();
  public readonly form: FormGroup;
  private readonly paySubject = new Subject<number>();

  constructor(
    @Optional() public readonly dialogRef: MatDialogRef<PayDialogComponent>,
    private readonly fb: FormBuilder,
    private readonly userBalanceService: UserBalance,
    private readonly snackBar: MatSnackBar,
  ) {
    this.form = fb.group({
      amount: [null, [Validators.required, Validators.min(1)]],
      confirmation: [null, [Validators.requiredTrue]]
    });
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  ngOnInit(): void {
    this.subscriptions.add(
      this.paySubject.pipe(
        tap(() => this.busy.next(true)),
        switchMap(amount => this.userBalanceService.addBalance(amount)),
      ).subscribe(success => {
        this.busy.next(false);
        if (success) {
          this.snackBar.open('Payment successful', 'Close', { duration: 5000 })
          this.dialogRef.close();
        }
      })
    );
  }

  onCloseClick(): void {
    this.dialogRef.close();
  }

  onPayClick(): void {
    this.submitted.next(true);
    if (this.form.valid) {
      this.paySubject.next(+this.form.value.amount);
    }
  }

  choose(amount: number): void {
    const control = this.form.get('amount')!;
    control.patchValue(`${amount}`);
    control.updateValueAndValidity();
  }
}
