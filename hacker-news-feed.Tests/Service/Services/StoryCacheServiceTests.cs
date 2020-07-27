using hacker_news_feed.Service.Interfaces.Story;
using hacker_news_feed.Service.Models.Item;
using hacker_news_feed.Service.Services;
using MemoryCache.Testing.Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace hacker_news_feed.Tests.Service.Services
{
    [TestFixture]
    class StoryCacheServiceTests
    {
        private IStoryCacheService _storyCacheService;

        [SetUp]
        public void SetUp()
        {
            var mockMemoryCache = Create.MockedMemoryCache();

            _storyCacheService = new StoryCacheService(mockMemoryCache);
        }

        [Test]
        public void AddStoryToCache_NewStory_ReturnsStory()
        {
            var story = new Item
            {
                Id = 1,
                Title = "Test",
                Type = ItemType.Story,
                Url = "https://www.google.com"
            };

            var result = _storyCacheService.AddStoryToCache(story);
            Assert.That(story, Is.EqualTo(result));
        }

        [Test]
        public void AddStoryToCache_Null_ReturnsNull()
        {
            Item story = null;

            var result = _storyCacheService.AddStoryToCache(story);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void AddNewStoryIdsToCache_Ids_ReturnsSameIdList()
        {
            var newStoryIds = new int[]{ 1, 2, 3 };

            var result = _storyCacheService.AddNewStoryIdsToCache(newStoryIds);
            Assert.That(newStoryIds, Is.EqualTo(result));
        }

        [Test]
        public void AddStoriesToCache_Stories_ReturnsSameStoryList()
        {
            var story_1 = new Item
            {
                Id = 1,
                Title = "Test",
                Type = ItemType.Story,
                Url = "https://www.google.com"
            };

            var story_2 = new Item
            {
                Id = 2,
                Title = "Test 2",
                Type = ItemType.Story,
                Url = "https://stackoverflow.com"
            };

            var stories = new List<Item> { story_1, story_2 };

            var result = _storyCacheService.AddStoriesToCache(stories);
            Assert.That(result, Has.Exactly(2).Items);

            var cachedStory_1 = result.GetValueOrDefault(story_1.Id);
            Assert.That(cachedStory_1, Is.EqualTo(story_1));

            var cachedStory_2 = result.GetValueOrDefault(story_2.Id);
            Assert.That(cachedStory_2, Is.EqualTo(story_2));
        }

        [Test]
        public void AddStoriesToCache_Null_ReturnsNull()
        {
            List<Item> stories = null;

            var result = _storyCacheService.AddStoriesToCache(stories);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetNewStoryIdsCache_Ids_ReturnsSavedIds()
        {
            var newStoryIds = new int[] { 1, 2, 3 };

            _ = _storyCacheService.AddNewStoryIdsToCache(newStoryIds);
            var result = _storyCacheService.GetNewStoryIdsCache();
            Assert.That(result, Is.EqualTo(newStoryIds));
        }

        [Test]
        public void GetNewStoryIdsCache_Null_ReturnsNull()
        {
            var result = _storyCacheService.GetNewStoryIdsCache();
            Assert.That(result, Is.Null);

            _ = _storyCacheService.AddNewStoryIdsToCache(null);
            result = _storyCacheService.GetNewStoryIdsCache();
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetStoriesCache_Stories_ReturnsStories()
        {
            var story_1 = new Item
            {
                Id = 1,
                Title = "Test",
                Type = ItemType.Story,
                Url = "https://www.google.com"
            };

            var story_2 = new Item
            {
                Id = 2,
                Title = "Test 2",
                Type = ItemType.Story,
                Url = "https://stackoverflow.com"
            };

            var stories = new List<Item> { story_1, story_2 };
            _ = _storyCacheService.AddStoriesToCache(stories);

            var result = _storyCacheService.GetStoriesCache();
            Assert.That(result, Has.Exactly(2).Items);

            var cachedStory_1 = result.GetValueOrDefault(story_1.Id);
            Assert.That(cachedStory_1, Is.EqualTo(story_1));

            var cachedStory_2 = result.GetValueOrDefault(story_2.Id);
            Assert.That(cachedStory_2, Is.EqualTo(story_2));
        }

        [Test]
        public void GetStoriesCache_Null_ReturnsNull()
        {
            var result = _storyCacheService.GetStoriesCache();
            Assert.That(result, Is.Null);

            _ = _storyCacheService.AddStoriesToCache(null);
            result = _storyCacheService.GetStoriesCache();
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetStoryCached_ValidId_ReturnsStory()
        {
            var story = new Item
            {
                Id = 1,
                Title = "Test",
                Type = ItemType.Story,
                Url = "https://www.google.com"
            };

            _ = _storyCacheService.AddStoryToCache(story);
            var result = _storyCacheService.GetStoryCached(story.Id);
            Assert.That(result, Is.EqualTo(story));
        }

        [Test]
        public void GetStoryCached_ValidIdFromList_ReturnsStory()
        {
            var story_1 = new Item
            {
                Id = 1,
                Title = "Test",
                Type = ItemType.Story,
                Url = "https://www.google.com"
            };

            var story_2 = new Item
            {
                Id = 2,
                Title = "Test 2",
                Type = ItemType.Story,
                Url = "https://www.google.com"
            };

            var stories = new List<Item> { story_1, story_2 };

            _ = _storyCacheService.AddStoriesToCache(stories);
            var cachedStory_1 = _storyCacheService.GetStoryCached(story_1.Id);
            Assert.That(cachedStory_1, Is.EqualTo(story_1));

            var cachedStory_2 = _storyCacheService.GetStoryCached(story_2.Id);
            Assert.That(cachedStory_2, Is.EqualTo(story_2));
        }

        [Test]
        public void GetStoriesNotContained_AllCached_ReturnsEmpty()
        {
            var story_1 = new Item
            {
                Id = 1,
                Title = "Test",
                Type = ItemType.Story,
                Url = "https://www.google.com"
            };

            var story_2 = new Item
            {
                Id = 2,
                Title = "Test 2",
                Type = ItemType.Story,
                Url = "https://www.google.com"
            };

            var stories = new List<Item> { story_1, story_2 };

            _ = _storyCacheService.AddStoriesToCache(stories);
            var notCached = _storyCacheService.GetStoriesNotContained(new List<int> { 1, 2 });
            Assert.That(notCached, Has.Exactly(0).Items);
        }

        [Test]
        public void GetStoriesNotContained_SomeCached_ReturnsNotAddedIds()
        {
            var story = new Item
            {
                Id = 1,
                Title = "Test",
                Type = ItemType.Story,
                Url = "https://www.google.com"
            };

            _ = _storyCacheService.AddStoryToCache(story);
            var notCached = _storyCacheService.GetStoriesNotContained(new List<int> { 1, 2 });
            Assert.That(notCached, Has.Exactly(1).Items);

            Assert.That(notCached.First(), Is.EqualTo(2));
        }

        [Test]
        public void GetStoriesNotContained_NoneCached_ReturnsAllIds()
        {
            var notCached = _storyCacheService.GetStoriesNotContained(new List<int> { 1, 2 });
            Assert.That(notCached, Has.Exactly(2).Items);
        }
    }
}
