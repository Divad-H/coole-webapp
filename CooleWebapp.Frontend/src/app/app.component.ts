import { OverlayContainer } from '@angular/cdk/overlay';
import { Component, HostBinding, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ThemeService } from './theme.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy{
  @HostBinding('class') className = '';

  private readonly subscriptions = new Subscription();

  constructor(
    private readonly themeService: ThemeService,
    private readonly overlay: OverlayContainer,
  ) {
  }

  ngOnInit(): void {
    this.subscriptions.add(
      this.themeService.darkMode.subscribe(darkMode => {
        const darkClassName = 'darkMode';
        this.className = darkMode ? darkClassName : ''

        if (darkMode) {
          this.overlay.getContainerElement().classList.add(darkClassName);
        } else {
          this.overlay.getContainerElement().classList.remove(darkClassName);
        }
      }));
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
