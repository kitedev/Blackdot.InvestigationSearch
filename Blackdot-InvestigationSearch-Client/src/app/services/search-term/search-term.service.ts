import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SearchTermService {

  constructor() { }

  private searchTermSubmitted: Subject<string> = new Subject();
  public searchTermSubmittedEvent = this.searchTermSubmitted.asObservable();

  public SearchTermSubmitted(value: string) 
  {
        this.searchTermSubmitted.next(value);
  }
}
