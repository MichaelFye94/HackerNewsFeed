using Microsoft.AspNetCore.Mvc;

namespace hacker_news_feed.Service.Models.Api
{
    public class ApiQueryModel
    {
        [FromQuery]
        public int Page { get; set; } = 1;

        [FromQuery]
        public int PageSize { get; set; } = 25;
    }
}
