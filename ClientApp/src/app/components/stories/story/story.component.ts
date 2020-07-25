import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { StoryService } from '../../../services/Story/story.service';

@Component({
  selector: 'app-story',
  templateUrl: './story.component.html',
  styleUrls: ['./story.component.css']
})
export class StoryComponent {
  id:number;
  story:Item;

  constructor(private route: ActivatedRoute, @Inject(StoryService) private storyService: StoryService) {
    this.id = this.route.snapshot.params.id;
    this.storyService.getItem(this.id).subscribe(item => this.story = item);
  }
}
