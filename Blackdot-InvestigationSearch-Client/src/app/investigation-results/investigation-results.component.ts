import { ChangeDetectorRef, Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SearchService } from '../services/search/search.service';
import { SearchResult } from '../types/search-result';
import { take } from 'rxjs/operators';
import { SearchTermService } from '../services/search-term/search-term.service';


@Component({
  selector: 'app-investigation-results',
  templateUrl: './investigation-results.component.html',
  styleUrls: ['./investigation-results.component.sass'],
  encapsulation: ViewEncapsulation.None,
})
export class InvestigationResultsComponent implements OnInit {
  displayedColumns: string[] = ['title', 'url', 'caption'];
  dataSource;
  isWait: boolean = false;
  displayResult: boolean = false;


  @ViewChild(MatSort, {static: false}) sort: MatSort;
  @ViewChild(MatPaginator, {static: false}) paginator: MatPaginator;

  constructor(private searchService:SearchService, private searchTermService:SearchTermService) {
  }

  ngOnInit(): void {


    this.searchTermService.searchTermSubmittedEvent.subscribe(searchTerm => {
      this.isWait = true;
      this.searchService.getSearchResults(searchTerm).pipe(take(1)).subscribe(result => {

          this.dataSource = new MatTableDataSource(result);
          this.displayResult = true;
          this.dataSource.sort = this.sort;
          this.dataSource.paginator = this.paginator;
          this.isWait = false;
      });
    })
  }

}
