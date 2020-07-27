using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace hacker_news_feed.Service.Models.Api
{
    public class ApiQueryModel
    {
        [FromQuery]
        [Range(1, 500)]
        public int Page { get; set; } = 1;

        [FromQuery]
        [Range(1, 500)]
        public int PageSize { get; set; } = 25;
    }
}
