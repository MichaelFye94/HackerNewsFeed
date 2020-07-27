import { PagedList } from '../../models/api/pagedList';
import { Item } from '../../models/item/Item';
import { getTestPagedList } from '../../models/testing/test-paged-list';
import { TestBed } from '@angular/core/testing';

import { StoryService } from './story.service';
import { asyncData } from '../../testing/async-observable-helpers';

describe('StoryService', () => {
  let service: StoryService;
  let httpClientSpy: {get: jasmine.Spy};
  
  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    service = new StoryService(<any>httpClientSpy, '/');
  });

  it('should return valid new stories (HttpClient called once)', () => {
    const expectedList = getTestPagedList();

    httpClientSpy.get.and.returnValue(asyncData(expectedList));
    service.getNewStories(1, 1).subscribe(
      stories => {
        expect(stories).toEqual(expectedList, 'expected list'),
        fail
      });
      expect(httpClientSpy.get.calls.count()).toBe(1, 'one call');
  });
});
