import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { BehaviorSubject, Subscriber } from "rxjs";


@Component({
  templateUrl: './pay-dialog.component.html',
  styleUrls: ['./pay-dialog.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PayDialogComponent implements OnInit, OnDestroy {

  public readonly busy = new BehaviorSubject(false);
  public readonly submitted = new BehaviorSubject(false);
  private readonly subscriptions = new Subscriber();
  public readonly form: FormGroup;

  constructor(
    public readonly dialogRef: MatDialogRef<PayDialogComponent>,
    private readonly fb: FormBuilder
  ) {
    this.form = fb.group({
      amount: [null, [Validators.required, Validators.min(1)]],
      confirmation: [null, [Validators.requiredTrue]]
    });
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  ngOnInit(): void { }

  onCloseClick(): void {
    this.dialogRef.close();
  }

  onPayClick(): void {
    this.submitted.next(true);
  }

  choose(amount: number): void {
    const control = this.form.get('amount')!;
    control.patchValue(`${amount}`);
    control.updateValueAndValidity();
  }
}
