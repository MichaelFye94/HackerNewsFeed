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

  it('should call get new stories and have initial page loaded', () => {
    const testList = getTestPagedList();
    fixture.detectChanges();
    fixture.whenStable().then(() => {
      expect(component.newStories).toEqual(testList.data);
      expect(component.page).toEqual(testList.page);
      expect(component.last).toEqual(testList.last);
    });
  });

  it('should onPageIncrement call getNewStories with page 2 after initial load', () => {
    fixture.whenStable().then(() => {
      spyOn(component, 'getNewStories');
      component.onPageIncrement();
      expect(component.getNewStories).toHaveBeenCalledWith(component.page + 1, component.pageSize);
    });
  });
  
  it('should onPageDecrement call getNewStories with page 1 if page is 2', () => {
    fixture.whenStable().then(() => {
      spyOn(component, 'getNewStories');
      component.last = 2;
      component.page = 2;
      component.onPageDecrement();
      expect(component.getNewStories).toHaveBeenCalledWith(component.page - 1, component.pageSize);
    })
  })

  it('should onPageIncrement not call getNewStories if page is same as last', () => {
    fixture.whenStable().then(() => {
      spyOn(component, 'getNewStories');
      component.page = component.last;
      component.onPageIncrement();
      expect(component.getNewStories).toHaveBeenCalledTimes(0);
    })
  })

  it('should onPageDecrement not call getNewStories after initial load', () => {
    fixture.whenStable().then(() => {
      spyOn(component, 'getNewStories');
      component.onPageDecrement();
      expect(component.getNewStories).toHaveBeenCalledTimes(0);
    })
  })
});
