import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StoryService } from 'src/app/services/Story/story.service';

import { Item } from '../item/Item';
import { PagedList } from '../api/pagedList';
import { getTestPagedList } from './test-paged-list';
import { getTestItems } from './test-items';
import { asyncData } from 'src/app/testing/async-observable-helpers';

export { Item } from '../item/Item';
export { PagedList } from '../api/pagedList';
export { getTestPagedList } from './test-paged-list';
export { getTestItems } from './test-items';

@Injectable()

export class TestStoryService extends StoryService {
    constructor(){
        super(null, null);
    }

    lastResult: Observable<any>;

    getNewStories(page: number = 1, pageSize: number = 25):Observable<PagedList<Item>> {
        let result = getTestPagedList();
        return this.lastResult = asyncData(result);
    }

    getItem(id: number):Observable<Item>{
        let result = getTestItems()[0];
        console.log("Service returning: ", result);
        return this.lastResult = asyncData(result);
    }
}