import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";

const toggledKey = 'sidenav.isToggled';

@Injectable()
export class SidenavService {

  private readonly isToggledSubject: BehaviorSubject<boolean>;
  public readonly isToggled: Observable<boolean>;

  constructor() {
    const isToggled = !!JSON.parse(localStorage.getItem(toggledKey) ?? 'true');
    this.isToggledSubject = new BehaviorSubject(isToggled);
    this.isToggled = this.isToggledSubject.asObservable();
  }

  toggle(state?: boolean | undefined) {
    const newState = state ?? !this.isToggledSubject.value;
    localStorage.setItem(toggledKey, JSON.stringify(newState));
    this.isToggledSubject.next(newState);
  }
}
