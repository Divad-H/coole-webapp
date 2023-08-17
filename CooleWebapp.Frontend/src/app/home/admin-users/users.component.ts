import { AfterViewInit, ChangeDetectionStrategy, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { merge, startWith, switchMap, Subscription, BehaviorSubject, catchError, of, map, distinctUntilChanged, debounceTime, combineLatest, skip, filter, empty, Subject, mapTo, Observable } from "rxjs";
import { MatSort, SortDirection } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';

import { CooleWebappApi } from "../../../generated/coole-webapp-api";
import { ConfirmDeleteUserComponent } from "./confirm-delete/confirm-delete-user.component";
import { BreakpointObserver, Breakpoints } from "@angular/cdk/layout";
import { UserDetailsComponent } from "./user-details/user-details.component";

@Component({
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UsersComponent implements AfterViewInit, OnDestroy {

  private readonly subscriptions = new Subscription();
  displayedColumns: Observable<string[]>;
  dataSubject = new BehaviorSubject<CooleWebappApi.IUserResponseModel[]>([]);
  data = this.dataSubject.asObservable();

  searchQuerySubject = new BehaviorSubject('');
  searchQuery = this.searchQuerySubject.pipe(distinctUntilChanged());

  private readonly refresh = new Subject()

  resultsLength = new BehaviorSubject(0);
  isLoadingResults = new BehaviorSubject(false);

  @ViewChild(MatPaginator) paginator?: MatPaginator;
  @ViewChild(MatSort) sort?: MatSort;

  constructor(
    private readonly adminUsers: CooleWebappApi.AdminUsersClient,
    private readonly matDialog: MatDialog,
    private readonly breakpointObserver: BreakpointObserver,
  ) {
    this.displayedColumns = merge(
      this.breakpointObserver
        .observe([
          Breakpoints.XSmall,
        ]).pipe(filter(b => b.matches), mapTo(0)),
      this.breakpointObserver
        .observe([
          Breakpoints.Small,
        ]).pipe(filter(b => b.matches), mapTo(1)),
      this.breakpointObserver
        .observe([
          Breakpoints.Medium,
        ]).pipe(filter(b => b.matches), mapTo(2)),
      this.breakpointObserver
        .observe([
          Breakpoints.Large,
          Breakpoints.XLarge,
        ]).pipe(filter(b => b.matches), mapTo(3))
    ).pipe(
      map(size => size == 3
        ? ['name', 'email', 'balance', 'administrator', 'fridge', 'buttons']
        : size == 2
          ? ['name', 'balance', 'administrator', 'fridge', 'buttons']
          : size == 1
            ? ['name', 'administrator', 'fridge', 'buttons']
            : ['name', 'buttons'])
    )
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  ngAfterViewInit(): void {
    const searchString = this.searchQuery.pipe(
      debounceTime(300)
    )

    this.subscriptions.add(
      merge(this.sort!.sortChange, this.searchQuery)
        .subscribe(() => (this.paginator!.pageIndex = 0))
    );

    this.subscriptions.add(
      combineLatest(
        this.sort!.sortChange.pipe(startWith({})),
        this.paginator!.page.pipe(startWith({})),
        searchString,
        this.refresh.pipe(startWith({}))
      )
        .pipe(
          switchMap(([_1, _2, search, _3]) => {
            this.isLoadingResults.next(true);
            return this.adminUsers.getUsers(
              this.paginator!.pageIndex,
              10,
              search == '' ? null : search,
              this.sort!.direction == "asc" ? 'ByNameAscending' : 'ByNameDescending'
            ).pipe(catchError(() => of(null)));
          }),
          map(data => {
            this.isLoadingResults.next(false);
            if (data === null) {
              return [];
            }
            this.resultsLength.next(data.pagination.totalItems);
            return data.users;
          }),
        )
        .subscribe(data => this.dataSubject.next(data)));
  }

  delete(user: CooleWebappApi.IUserResponseModel) {
    const modalRef = this.matDialog.open(ConfirmDeleteUserComponent, {
      data: { ...user }
    });
    modalRef.afterClosed().pipe(
      filter(v => v),
      switchMap(() => this.adminUsers.deleteUser(user.id).pipe(
        catchError(e => {
          console.error(e);
          return of({});
        })
      ))
    ).subscribe(() => {
      this.refresh.next({});
    })
  }

  edit(user: CooleWebappApi.IUserResponseModel) {
    const modalRef = this.matDialog.open(UserDetailsComponent, {
      data: { user: user }
    });
    modalRef.afterClosed().subscribe(user => {
      if (user) {
        this.refresh.next({});
      }
    });
  }

  formatBalance(balance: number) {
    return balance.toFixed(2);
  }

  isAdmin(roles: CooleWebappApi.UserRole[]) {
    return roles.includes('Administrator');
  }

  isFridge(roles: CooleWebappApi.UserRole[]) {
    return roles.includes('Fridge');
  }
}
