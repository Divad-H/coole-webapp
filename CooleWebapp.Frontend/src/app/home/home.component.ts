import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, shareReplay } from "rxjs";
import { AuthService } from "../auth/auth.service";
import { SidenavService } from "./sidenav/sidenav.service";
import { MediaMatcher } from '@angular/cdk/layout';

@Component({
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HomeComponent implements OnDestroy {

  public readonly isSidenavToggled: Observable<boolean>;
  public readonly mobileQuery: MediaQueryList;

  private _mobileQueryListener: () => void;

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router,
    readonly sidenavService: SidenavService,
    private readonly changeDetectorRef: ChangeDetectorRef,
    private readonly media: MediaMatcher,
  ) {

    this.isSidenavToggled = sidenavService.isToggled;

    this.mobileQuery = media.matchMedia('(max-width: 600px)');
    this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addListener(this._mobileQueryListener);
  }

  ngOnDestroy(): void {
    this.mobileQuery.removeListener(this._mobileQueryListener);
  }

  sidenavToggled(value: boolean) {
    this.sidenavService.toggle(value);
  }
}
