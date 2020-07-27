import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { StoryComponent } from './story.component';
import { StoryService } from 'src/app/services/Story/story.service';
import { TestStoryService, getTestItems } from '../../../models/testing/test-story.service';


import { ActivatedRoute } from '@angular/router';

describe('StoryComponent', () => {
  let component: StoryComponent;
  let fixture: ComponentFixture<StoryComponent>;


  beforeEach(async() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: {
                id: () => 1
              }
            }
          }
        },
        {provide: StoryService, useClass: TestStoryService}
      ],
      declarations: [StoryComponent]
    });
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  })

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  // it('should load story from service', () => {
  //   const testStory = getTestItems()[0];
  //   component.ngOnInit();
  //   expect(component.id).toBeTruthy();
  //   expect(component.story).toEqual(testStory);
  // });
});
