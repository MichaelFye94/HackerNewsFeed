import { PagedList } from '../api/pagedList';
import { Item } from '../item/Item';
import { getTestItems } from '../testing/test-items';

export function getTestPagedList(): PagedList<Item> {
    let testItems = getTestItems();
    return {
        data: testItems,
        page: 1,
        last: 2,
        previous: 1,
        next: 2
      }
}
