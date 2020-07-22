import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public newStories: NewStory[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<NewStory[]>(baseUrl + 'story').subscribe(result => {
      this.newStories = result;
    }, error => console.error(error));
  }
}

interface NewStory {
  id: number;
}
