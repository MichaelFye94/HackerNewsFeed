import { Item } from '../item/Item';

export function getTestItems(): Item[] {
    return [
        {
            id: "1",
            title: "Test",
            by: "user",
            url: "https://www.google.com"
        }
    ]
}