using hacker_news_feed.Service.Interfaces.Story;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Reflection.Metadata.Ecma335;

namespace hacker_news_feed.Service.Services
{
    public class StoryCacheService : IStoryCacheService
    {
        private IMemoryCache _cache;
        private MemoryCacheEntryOptions _defaultOptions, _extendedExpirationOptions;

        public StoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
            _defaultOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(1));
            _extendedExpirationOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(15));
        }

        public bool TryGetValue<T>(object key, out T value)
            => _cache.TryGetValue(key, out value);

        /// <summary>
        /// Sets cache item with default cache entry options
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public T Set<T>(object key, T value)
            => _cache.Set(key, value, _defaultOptions);

        /// <summary>
        /// Sets cache item with extended life-span cache entry options
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public T SetExtendedExpiration<T>(object key, T value)
            => _cache.Set(key, value, _extendedExpirationOptions);
    }
}