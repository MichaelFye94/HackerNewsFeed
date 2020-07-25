using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hacker_news_feed.Service.Interfaces.Story
{
    public interface IStoryCacheService
    {
        bool TryGetValue<T>(object key, out T value);
        T Set<T>(object key, T value);
        T SetExtendedExpiration<T>(object key, T value);
    }
}
