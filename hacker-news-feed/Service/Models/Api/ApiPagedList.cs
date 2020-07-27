using System.Collections.Generic;

namespace hacker_news_feed.Service.Models.Api
{
    public class ApiPagedList<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int Page { get; set; }
        public int Last { get; set; }
        public int Previous { get; set; }
        public int Next { get; set; }
        public ApiPagedList(IEnumerable<T> data, int page, int last)
        {
            Data = data;
            Page = page;
            Last = last;
            Previous = page > 0 ? page - 1 : 0;
            Next = page < last ? page + 1 : last;
        }
    }
}
