import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms'

import { SearchComponent } from './search.component';
import { Router } from '@angular/router';

describe('SearchComponent', () => {
  let component: SearchComponent;
  let fixture: ComponentFixture<SearchComponent>;

  beforeEach(async(() => {
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    TestBed.configureTestingModule({
      imports:[FormsModule],
      declarations: [ SearchComponent ],
      providers:[
        {provide: Router, useValue: routerSpy}
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should navigate with id', () => {
    component.id = 1;
    component.onSearch();
    const routerSpy = fixture.debugElement.injector.get(Router);
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/1']);
  });

  it('should not navigate with falsey id', () => {
    component.id = undefined;
    component.onSearch();
    const routerSpy = fixture.debugElement.injector.get(Router);
    expect(routerSpy.navigate).toHaveBeenCalledTimes(0);
  });
});
