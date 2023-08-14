import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";


@Injectable()
export class ThemeService {
  private readonly darkModeSub = new BehaviorSubject(localStorage.getItem('darkMode') === 'darkMode');
  public darkMode: Observable<boolean> = this.darkModeSub;

  public setDarkMode(value: boolean) {
    localStorage.setItem('darkMode', value ? 'darkMode' : '');
    this.darkModeSub.next(value);
  }

}
