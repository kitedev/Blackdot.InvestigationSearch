import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SearchResult } from '../../types/search-result';

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor(private http: HttpClient) { }

  getSearchResults(searchTerm:string) : Observable<SearchResult[]>
  {
    let url = "https://localhost:44375/api/search?q=" + encodeURIComponent(searchTerm);

    return this.http.get<SearchResult[]>(url);
  }
}
