import { Item } from '../../models/item/Item';
import { PagedList } from '../../models/api/pagedList';
import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StoryService {
  private endpoint:string = 'story';
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getNewStories(page: number = 1, pageSize: number = 25):Observable<PagedList<Item>> {
    return this.http.get<PagedList<Item>>(`${this.baseUrl}${this.endpoint}?page=${page}&pageSize=${pageSize}`);
  }

  getItem(id: number):Observable<Item> {
    return this.http.get<Item>(this.baseUrl + `${this.endpoint}/${id}`);
  }
}
