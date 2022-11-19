import { AfterViewInit, ChangeDetectionStrategy, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { merge, startWith, switchMap, Subscription, BehaviorSubject, catchError, of, map, distinctUntilChanged, debounceTime, combineLatest, skip } from "rxjs";
import { MatSort, SortDirection } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';

import { CooleWebappApi } from "../../../generated/coole-webapp-api";

@Component({
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductsComponent implements AfterViewInit, OnDestroy {

  private readonly subscriptions = new Subscription();
  displayedColumns: string[] = ['name', 'description', 'price', 'buttons'];
  dataSubject = new BehaviorSubject<CooleWebappApi.Product[]>([]);
  data = this.dataSubject.asObservable();

  searchQuerySubject = new BehaviorSubject('');
  searchQuery = this.searchQuerySubject.pipe(distinctUntilChanged());

  resultsLength = new BehaviorSubject(0);
  isLoadingResults = new BehaviorSubject(false);

  @ViewChild(MatPaginator) paginator?: MatPaginator;
  @ViewChild(MatSort) sort?: MatSort;

  constructor(private readonly adminProducts: CooleWebappApi.AdminProductsClient) { }

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
      combineLatest(this.sort!.sortChange.pipe(startWith({})), this.paginator!.page.pipe(startWith({})), searchString)
        .pipe(
          switchMap(([_1, _2, search]) => {
            this.isLoadingResults.next(true);
            return this.adminProducts.getProducts(
              this.paginator!.pageIndex,
              10,
              search == '' ? null : search,
              null,
              this.sort!.direction == "asc" ? 0 : 1
            ).pipe(catchError(() => of(null)));
          }),
          map(data => {
            this.isLoadingResults.next(false);
            if (data === null) {
              return [];
            }
            this.resultsLength.next(data.pagination.totalItems);
            return data.products;
          }),
        )
        .subscribe(data => this.dataSubject.next(data)));
  }
}
