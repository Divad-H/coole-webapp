import { AfterViewInit, ChangeDetectionStrategy, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { merge, startWith, switchMap, Subscription, BehaviorSubject, catchError, of, map, distinctUntilChanged, debounceTime, combineLatest, skip, filter, empty, Subject, mapTo, Observable } from "rxjs";
import { MatSort, SortDirection } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';

import { CooleWebappApi } from "../../../generated/coole-webapp-api";
import { ConfirmDeleteComponent } from "./confirm-delete/confirm-delete.component";
import { ProductDetailsComponent } from "./product-details/product-details.component";
import { BreakpointObserver, Breakpoints } from "@angular/cdk/layout";

@Component({
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductsComponent implements AfterViewInit, OnDestroy {

  private readonly subscriptions = new Subscription();
  displayedColumns: Observable<string[]>;
  dataSubject = new BehaviorSubject<CooleWebappApi.IProductResponseModel[]>([]);
  data = this.dataSubject.asObservable();

  searchQuerySubject = new BehaviorSubject('');
  searchQuery = this.searchQuerySubject.pipe(distinctUntilChanged());

  private readonly refresh = new Subject()

  resultsLength = new BehaviorSubject(0);
  isLoadingResults = new BehaviorSubject(false);

  @ViewChild(MatPaginator) paginator?: MatPaginator;
  @ViewChild(MatSort) sort?: MatSort;

  constructor(
    private readonly adminProducts: CooleWebappApi.AdminProductsClient,
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
          Breakpoints.Large,
          Breakpoints.XLarge,
        ]).pipe(filter(b => b.matches), mapTo(2))
    ).pipe(
      map(size => size == 2
        ? ['name', 'description', 'price', 'state', 'buttons']
        : size == 1
          ? ['name', 'price', 'state', 'buttons']
          : ['name', 'price', 'buttons'])
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
            return this.adminProducts.getProducts(
              this.paginator!.pageIndex,
              10,
              search == '' ? null : search,
              null,
              this.sort!.direction == "asc" ? 'ByNameAscending' : 'ByNameDescending'
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

  delete(product: CooleWebappApi.IProductResponseModel) {
    const modalRef = this.matDialog.open(ConfirmDeleteComponent, {
      data: { ...product }
    });
    modalRef.afterClosed().pipe(
      filter(v => v),
      switchMap(() => this.adminProducts.deleteProduct(product.id).pipe(
        catchError(e => {
          console.error(e);
          return of({});
        })
      ))
    ).subscribe(() => {
      this.refresh.next({});
    })
  }

  edit(product: CooleWebappApi.IProductResponseModel) {
    const modalRef = this.matDialog.open(ProductDetailsComponent, {
      data: { product: product }
    });
    modalRef.afterClosed().subscribe(product => {
      if (product) {
        this.refresh.next({});
      }
    });
  }

  add() {
    const modalRef = this.matDialog.open(ProductDetailsComponent, {
      data: {}
    });
    modalRef.afterClosed().subscribe(product => {
      if (product) {
        this.refresh.next({});
      }
    });
  }

  formatPrice(price: number) {
    return price.toFixed(2);
  }
}
