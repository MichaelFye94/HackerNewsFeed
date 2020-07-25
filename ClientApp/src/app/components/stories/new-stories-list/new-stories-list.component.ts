import { Component, Inject, OnInit } from '@angular/core';
import { StoryService } from '../../../services/Story/story.service';

@Component({
  selector: 'app-new-stories-list',
  templateUrl: './new-stories-list.component.html',
  styleUrls: ['./new-stories-list.component.css']
})
export class NewStoriesListComponent implements OnInit {

  newStories:Item[];
  listPages:number[];
  last:number;
  next:number;
  page:number;
  pageSize:number;
  previous:number;
  constructor(@Inject(StoryService) private storyService: StoryService) {
    this.page = 1;
    this.pageSize = 10;
  }

  ngOnInit() {
    this.getNewStories(this.page, this.pageSize);
  }

  onChangePage(event: any) {
    this.getNewStories(event.target.value, this.pageSize);
  }

  onPageDecrement() {
    if(this.page > 1) {
      this.getNewStories(this.page - 1, this.pageSize);
    }
  }

  onPageIncrement() {
    if(this.page < this.last) {
      this.getNewStories(this.page + 1, this.pageSize);
    }
  }

  private getNewStories(page:number, pageSize:number) {
    this.storyService.getNewStories(page, pageSize).subscribe(stories => this.updatePage(stories));
  }

  private updatePage(stories: PagedList<Item>) {
    this.newStories = stories.data;
    this.page = stories.page;
    this.last = stories.last;
    this.next = stories.next;
    this.previous = stories.previous;
  }

  private calculateListPages() {
    let index:number = this.page > 1 ? Math.min(1, this.page - 4) : 1;
    this.listPages = [];
    for(let i = 0; i < 10; i++) {
      this.listPages.push(index++);
      if(index >= this.last) return;
    }
  }

}
