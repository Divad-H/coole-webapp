import { AfterViewInit, Component, Input, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatSnackBar } from "@angular/material/snack-bar";
import { ActivatedRoute, Router } from "@angular/router";
import { BehaviorSubject, Subject, Subscription, Observable, tap, withLatestFrom, switchMap, of, catchError, map } from "rxjs";
import { CooleWebappApi } from "../../../../generated/coole-webapp-api";

@Component({
  selector: 'app-pin-pad',
  templateUrl: './pin-pad.component.html',
  styleUrls: ['pin-pad.component.css']
})
export class PinPadComponent implements AfterViewInit, OnDestroy {

  private readonly subscriptions = new Subscription();
  public readonly numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9];
  public readonly form: FormGroup;

  private readonly submit = new Subject<string>();
  public readonly busy = new BehaviorSubject(false);

  @Input() public selectedProduct: Observable<CooleWebappApi.IProductResponseModel> | undefined;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly snackBar: MatSnackBar,
  ) {
    this.form = formBuilder.group({
      pinCode: [null, Validators.required]
    });
  }

  ngAfterViewInit(): void {
    this.submit.pipe(
      tap(() => this.busy.next(true)),
      withLatestFrom(this.selectedProduct!),
      switchMap(([pinCode, product]) => of(true).pipe(
        map(() => product.name),
        catchError(err => {
          this.snackBar.open(err.message ?? 'An error occured.', 'Close', { duration: 5000 });
          return of(null);
        })
      ))
    ).subscribe(boughtProduct => {
      this.busy.next(false);
      if (boughtProduct != null) {
        this.router.navigate(['..'], { relativeTo: this.route });
        this.snackBar.open(`Enjoy your ${boughtProduct}!`, 'Close', { duration: 5000 });
      }
    })
  }

  numberClicked(number: number) {
    this.form.patchValue({
      pinCode: `${this.form.value.pinCode ?? ''}${number}`
    });
  }

  backspaceClicked() {
    const code: string = this.form.value.pinCode;
    if (code?.length) {
      this.form.patchValue({
        pinCode: code.substring(0, code.length - 1)
      });
    }
  }

  submitClicked() {
    if (this.form.valid) {
      this.submit.next(this.form.value.pinCode);
    }
  }

  cancelClicked() {
    this.router.navigate(['..'], { relativeTo: this.route });
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
