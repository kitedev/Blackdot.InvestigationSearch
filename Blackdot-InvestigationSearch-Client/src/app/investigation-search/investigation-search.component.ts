import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import {MatInputModule} from '@angular/material/input';
import { SearchTermService } from '../services/search-term/search-term.service';

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}

@Component({
  selector: 'app-investigation-search',
  templateUrl: './investigation-search.component.html',
  styleUrls: ['./investigation-search.component.sass']
})
export class InvestigationSearchComponent implements OnInit {

  searchTerm: string;
  searchForm: FormGroup;
  matcher = new MyErrorStateMatcher();

  constructor(private searchTermService:SearchTermService) { }

  ngOnInit(): void {
    this.searchForm = new FormGroup(
      {
        'search':new FormControl('', [Validators.required, Validators.minLength(3)])
      });

  }

  search(): void {
      console.log("search submitted: " + this.searchTerm);
      this.searchTermService.SearchTermSubmitted(this.searchTerm);
  }

}
