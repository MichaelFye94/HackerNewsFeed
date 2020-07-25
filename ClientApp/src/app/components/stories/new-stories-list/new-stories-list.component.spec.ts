import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewStoriesListComponent } from './new-stories-list.component';

describe('NewStoriesListComponent', () => {
  let component: NewStoriesListComponent;
  let fixture: ComponentFixture<NewStoriesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewStoriesListComponent ]
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
