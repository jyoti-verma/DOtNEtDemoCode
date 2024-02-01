import { ChangeDetectorRef, Component, ElementRef, Input, Renderer2, RendererStyleFlags2, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { startWith, switchMap, map } from 'rxjs/operators';
import { merge } from 'rxjs';
import { SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
  encapsulation: ViewEncapsulation.None,

})
export class TableComponent {
  @Input() DisplayedColumn: any;
  @Input() DataSource: any;
  @Input() columnMappings: any;
  @Input() SortDisabled: any;
  @Input() SearchDisabled: any;
  @Input() pageSize: any;
  resultsLength = 0;
  selection = new SelectionModel<any>(true, []);
  selectedRow: any | null = null;
  dataSource = new MatTableDataSource<any>([]);
  @ViewChild('sort') matSort = new MatSort();
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;

  constructor(private el: ElementRef, private renderer: Renderer2, private cdr: ChangeDetectorRef) {

  }
  ngOnInit() {
    this.matSort.disabled = this.SortDisabled;
    this.dataSource.data = this.DataSource;
    this.dataSource.paginator = this.paginator;
    this.paginator.pageIndex = 0; // Optional: Reset pageIndex to 0 when the component loads
  }
  ngAfterViewInit() {
    this.dataSource.sort = this.matSort;

    if (this.SearchDisabled) {
      this.applyStyles();
    }
    this.loadData();
    this.addPlusIcon();
  }

  addPlusIcon() {
    const iconClass = 'material-icons';
    const iconName = 'add';
    const lastCol = this.el.nativeElement.querySelectorAll('.cdk-column-plus');
    lastCol.forEach((element: any, i: any) => {
      const iconClass2 = 'plus-icon' + "-" + i;
      if (i != 0) {
        const iconElement = this.renderer.createElement('mat-icon');
        this.renderer.addClass(iconElement, iconClass);
        this.renderer.addClass(iconElement, iconClass2);
        const textNode = this.renderer.createText(iconName);
        this.renderer.appendChild(iconElement, textNode);
        this.renderer.appendChild(element, iconElement);
        this.renderer.setStyle(iconElement, 'margin-left', '105px', RendererStyleFlags2.Important);
        this.renderer.setStyle(iconElement, 'color', '#8d8989', RendererStyleFlags2.Important);
        this.renderer.setStyle(iconElement, 'font-size', '20px', RendererStyleFlags2.Important);
      }
    });
  }
  //Apply styles
  private applyStyles() {
    const element = this.el.nativeElement.querySelectorAll('.table-search');

    if (element.length >= 0) {
      element.forEach((element: any) => {
        this.renderer.setStyle(element, 'display', 'none', RendererStyleFlags2.Important);
      });
    }
    const sort = this.el.nativeElement.querySelectorAll('.mat-sort-header-content');
    const sortArrow = this.el.nativeElement.querySelectorAll('.mat-sort-header-arrow');


    if (sort.length >= 0) {
      sort.forEach((i: any) => {
        this.renderer.setStyle(i, 'width', '100%', RendererStyleFlags2.Important);
      });
    }
    if (sortArrow.length >= 0) {
      sortArrow.forEach((i: any) => {
        this.renderer.setStyle(i, 'margin-bottom', '0px', RendererStyleFlags2.Important);

      });
    }

  }
  //For searching
  handleMouseEnter(event: Event) {
    this.matSort.disabled = !(event.target as HTMLInputElement).value;
  }
  handleMouseLeave(event: Event) {
    this.matSort.disabled = !!(event.target as HTMLInputElement).value;
  }
  applyFilter(event: Event) {
    this.dataSource.data = this.DataSource;
    const filterValue = (event.target as HTMLInputElement).value;
    if (filterValue == "") {
      this.dataSource.data = this.DataSource;
    } else {
      const columnName = (event.target as HTMLInputElement).classList[2];
      const columnIndex = this.dataSource.data.length > 0 ? Object.keys(this.dataSource.data[0]).indexOf(columnName) : -1;
      var filterRow = this.dataSource.data.filter(row => row[Object.keys(row)[columnIndex]].toString().toLowerCase().includes(filterValue.toLowerCase()));
      this.dataSource.data = filterRow;
    }
  }

  onRowClick(row: any) {
    this.selectedRow = this.selectedRow === row ? null : row;
  }
  //For Pagination
  loadData() {
    merge(this.paginator.page)
      .pipe(
        startWith({}),
        switchMap(() => {
          return this.dataSource.data
        }),
        map(data => {
          this.resultsLength = data.total; // Assuming your server sends the total number of records
          return data.items; // Assuming your server sends an array of items
        })
      )
  }
}
