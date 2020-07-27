import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewStoriesListComponent } from './new-stories-list.component';
import { StoryService } from 'src/app/services/Story/story.service';
import { getTestPagedList, TestStoryService } from '../../../models/testing/test-story.service';

describe('NewStoriesListComponent', () => {
  let component: NewStoriesListComponent;
  let fixture: ComponentFixture<NewStoriesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewStoriesListComponent ],
      providers: [
        {
          provide: StoryService, useClass: TestStoryService
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewStoriesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
