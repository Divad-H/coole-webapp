import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from "@angular/core";
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";
import { BehaviorSubject, catchError, combineLatest, EMPTY, empty, filter, from, Observable, of, Subject, Subscription, switchMap, take, tap } from "rxjs";
import { CooleWebappApi } from "../../../generated/coole-webapp-api";
import { ParentErrorStateMatcher } from "../../utilities/error-state-matchers";


@Component({
  templateUrl: 'settings-dialog.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styleUrls: ['settings-dialog.component.css']
})
export class SettingsDialogComponent implements OnInit, OnDestroy {

  private readonly subscription = new Subscription();
  public readonly busy = new BehaviorSubject(true);
  public readonly form: FormGroup;
  readonly parentErrorStateMatcher = new ParentErrorStateMatcher();
  public readonly hasOldPinCode = new BehaviorSubject(false);
  private readonly okSubject = new Subject<any>();

  public readonly permissionSettings: CooleWebappApi.BuyOnFridgePermission[] = [
    "NotPermitted", "AlwaysPermitted", "WithPinCode"
  ];

  public readonly permissionText = new Map<CooleWebappApi.BuyOnFridgePermission, string>([
    ["NotPermitted", "Not permitted"],
    ["AlwaysPermitted", "Permitted without pin code"],
    ["WithPinCode", "Permitted with pin code"]
  ])

  private readonly pinCodeValidators = [Validators.minLength(4), Validators.pattern("[0-9]*")];
  private readonly formValidators = [this.pinsMatch, this.pinRequired];

  constructor(
    public readonly dialogRef: MatDialogRef<SettingsDialogComponent>,
    fb: FormBuilder,
    private readonly userSettingsClient: CooleWebappApi.UserSettingsClient,
    private readonly snackBar: MatSnackBar,
  ) {

    this.form = fb.group({
      buyOnFridgePermission: ["NotPermitted", [Validators.required]],
      pinCode: [{ value: null, disabled: true }, { validators: this.pinCodeValidators }],
      confirmPinCode: [{ value: null, disabled: true }]
    }, { validators: this.formValidators });
  }

  ngOnInit(): void {
    const pinCode = this.form.get('pinCode')!;
    const confirmPinCode = this.form.get('confirmPinCode')!;

    this.subscription.add(
      combineLatest(
        [this.hasOldPinCode,
        this.form.get('buyOnFridgePermission')!.valueChanges])
        .subscribe(([hasOldPinCode, permission]) => {
          if (permission == "WithPinCode") {
            if (!hasOldPinCode) {
              confirmPinCode.enable();
            }
            pinCode.enable();
          } else {
            pinCode.disable();
            confirmPinCode.disable();
          }
        }));
      
    this.subscription.add(
      from(this.userSettingsClient.getSettings())
        .pipe(
          catchError(() => { this.busy.next(false); return EMPTY; }),
        )
        .subscribe(settings => {
          if (settings.buyOnFridgePermission == "WithPinCode") {
            this.form.clearValidators();
            pinCode.clearValidators();
            pinCode.patchValue('');
            confirmPinCode.patchValue('***');
            confirmPinCode.disable();

            this.hasOldPinCode.next(true);

            this.subscription.add(
              pinCode.valueChanges.pipe(
                filter(v => v?.length),
                take(1)
              ).subscribe(v => {
                pinCode.addValidators(this.pinCodeValidators);
                this.form.addValidators(this.formValidators);
                confirmPinCode.patchValue(null);
                confirmPinCode.enable();
                this.form.updateValueAndValidity();
                this.hasOldPinCode.next(false);
              })
            );
          }
          this.form.get('buyOnFridgePermission')!.patchValue(settings.buyOnFridgePermission);
          this.form.updateValueAndValidity();
          this.busy.next(false);
        })
    );

    this.subscription.add(
      combineLatest(
        [this.okSubject, this.hasOldPinCode]
      ).pipe(
        filter(() => this.form.valid),
        tap(() => this.busy.next(true)),
        switchMap(([value, hasOldPinCode]) => {

          let requestData: CooleWebappApi.UpdateSettingsRequestModel;
          if (value.buyOnFridgePermission == "WithPinCode" && !hasOldPinCode) {
            requestData = new CooleWebappApi.UpdateSettingsRequestModel({
              buyOnFridgePermission: value.buyOnFridgePermission,
              buyOnFridgePinCode: value.pinCode
            })
          } else {
            requestData = new CooleWebappApi.UpdateSettingsRequestModel({
              buyOnFridgePermission: value.buyOnFridgePermission
            })
          }
          return from(this.userSettingsClient.updateSettings(requestData)).pipe(
            catchError(e => {
              this.snackBar.open(e.message ?? 'Could not apply settings.', 'Close', { duration: 5000 });
              return of({ error: true });
            })
          );
        }),
      ).subscribe(res => {
        this.busy.next(false);
        if (res?.error != true) {
          this.snackBar.open('Settings successfully changed.', 'Close', { duration: 5000 });
          this.dialogRef.close();
        }
      }));
  }

  onOkClick() {
    this.okSubject.next(this.form.value);
  }

  onCloseClick() {
    this.dialogRef.close();
  }

  pinsMatch(control: AbstractControl): ValidationErrors | null {
    const group = control as FormGroup;
    const buyOnFridgePermission: CooleWebappApi.BuyOnFridgePermission | null = group.get('buyOnFridgePermission')!.value;
    if (buyOnFridgePermission == null || buyOnFridgePermission != "WithPinCode")
      return null;

    const pinCode = group.get('pinCode')!.value;
    const confirmPinCode = group.get('confirmPinCode')!.value;

    return pinCode === confirmPinCode
      ? null
      : { pinsMatch: true };
  }

  pinRequired(control: AbstractControl): ValidationErrors | null {
    const group = control as FormGroup;
    const buyOnFridgePermission: CooleWebappApi.BuyOnFridgePermission | null = group.get('buyOnFridgePermission')!.value;
    if (buyOnFridgePermission == null || buyOnFridgePermission != "WithPinCode")
      return null;

    const pinCode = group.get('pinCode')!.value;

    return pinCode?.length > 0
      ? null
      : { pinRequired: true };
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

}
