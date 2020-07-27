export interface PagedList<T> {
    data: T[];
    page: number;
    last: number;
    previous: number;
    next: number;
}