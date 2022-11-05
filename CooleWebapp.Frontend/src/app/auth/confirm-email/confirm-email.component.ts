import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { BehaviorSubject, catchError, mapTo, of, Subscription, switchMap, throwError } from "rxjs";

import { CooleWebappApi } from "../../../generated/coole-webapp-api";

@Component({
  selector: 'cw-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['../auth-styles.css'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class ConfirmEmailComponent implements OnInit, OnDestroy{

  private readonly subscriptions = new Subscription();
  private readonly resultSubject = new BehaviorSubject<{error?: string} | null>(null);
  readonly result = this.resultSubject.asObservable();

  constructor(
    private readonly registration: CooleWebappApi.RegistrationClient,
    public readonly route: ActivatedRoute,
  ) {

  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  ngOnInit(): void {
    this.subscriptions.add(
      this.route.queryParams.pipe(
        switchMap(params => (!params['email'] || !params['token']
          ? throwError(new Error('Invalid link, please check that the link is correct.'))
          : this.registration.confirmEmail(params['token'], params['email']))),
        mapTo({}),
        catchError(e => {
          if (e.message) {
            this.resultSubject.next(e.message);
          }
          return of({ error: e.message ?? 'An error occured' });
        }),
      ).subscribe(res => {
        this.resultSubject.next(res);
      })
    );
  }

}
