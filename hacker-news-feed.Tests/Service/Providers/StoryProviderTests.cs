using hacker_news_feed.Service.Interfaces.Story;
using hacker_news_feed.Service.Models.Item;
using hacker_news_feed.Service.Providers;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hacker_news_feed.Tests.Service.Providers
{
    [TestFixture]
    class StoryProviderTests
    {
        private Mock<IStoryService> _mockStoryService;
        private Mock<IStoryCacheService> _mockCache;
        private IStoryProvider _storyProvider;

        [SetUp]
        public void SetUp()
        {
            _mockStoryService = new Mock<IStoryService>();

            IEnumerable<int> newStories = new List<int> { 1, 2, 3 };
            var story = new Item
            {
                Id = 1,
                Title = "Test"
            };
            var story2 = new Item
            {
                Id = 2,
                Title = "Test 2"
            };
            var story_cached = new Item
            {
                Id = 3,
                Title = "Test 3"
            };
            IEnumerable<Item> stories = new List<Item> { story, story2 };
            SortedList<int, Item> storiesAfterCache = new SortedList<int, Item>();
            storiesAfterCache.Add(story.Id, story);
            storiesAfterCache.Add(story2.Id, story2);
            storiesAfterCache.Add(story_cached.Id, story_cached);

            _mockStoryService.Setup(x => x.GetNewStories())
                .Returns(Task.FromResult(newStories));
            _mockStoryService.Setup(x => x.GetItem(story.Id))
                .Returns(Task.FromResult(story));
            _mockStoryService.Setup(x => x.GetItem(story2.Id))
                .Returns(Task.FromResult(story2));
            _mockStoryService.Setup(x => x.GetItem(It.IsNotIn(new int[] { 1, 2 })))
                .Returns(Task.FromResult<Item>(null));
            _mockStoryService.Setup(x => x.GetItems(new List<int> { 1, 2 }))
                .Returns(Task.FromResult(stories));

            _mockCache = new Mock<IStoryCacheService>();

            IEnumerable<int> cachedStory = new List<int> { 3 };
            SortedList<int, Item> cachedStories = new SortedList<int, Item>();
            cachedStories.Add(story_cached.Id, story_cached);

            _mockCache.Setup(x => x.GetStoryCached(story_cached.Id))
                .Returns(story_cached);
            _mockCache.Setup(x => x.GetStoryCached(It.IsNotIn(cachedStory)))
                .Returns<Item>(null);
            _mockCache.Setup(x => x.GetStoriesCache())
                .Returns(cachedStories);
            _mockCache.Setup(x => x.GetNewStoryIdsCache())
                .Returns<IEnumerable<int>>(null);
            _mockCache.Setup(x => x.GetStoriesNotContained(cachedStory))
                .Returns(new List<int>());
            _mockCache.Setup(x => x.GetStoriesNotContained(newStories))
                .Returns(new List<int> { 1, 2 });
            _mockCache.Setup(x => x.AddStoryToCache(story))
                .Returns(story);
            _mockCache.Setup(x => x.AddStoryToCache(story2))
                .Returns(story2);
            _mockCache.Setup(x => x.AddStoriesToCache(It.IsAny<IEnumerable<Item>>()))
                .Returns(storiesAfterCache);
            _mockCache.Setup(x => x.AddNewStoryIdsToCache(newStories))
                .Returns(newStories);

            _storyProvider = new StoryProvider(_mockStoryService.Object, _mockCache.Object);
        }

        [Test]
        public async Task GetItem_NotCached_ReturnsItem()
        {
            var expectedStory = new Item
            {
                Id = 1,
                Title = "Test"
            };

            var result = await _storyProvider.GetItem(expectedStory.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(expectedStory.Id));
            Assert.That(result.Title, Is.EqualTo(expectedStory.Title));
            _mockStoryService.Verify(x => x.GetItem(expectedStory.Id), Times.Once);
        }

        [Test]
        public async Task GetItem_Cached_ReturnsItemFromCache()
        {
            var expectedStory = new Item
            {
                Id = 3,
                Title = "Test 3"
            };

            var result = await _storyProvider.GetItem(expectedStory.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(expectedStory.Id));
            Assert.That(result.Title, Is.EqualTo(expectedStory.Title));
            _mockStoryService.Verify(x => x.GetItem(expectedStory.Id), Times.Never);
        }

        [Test]
        public async Task GetItem_Invalid_ReturnsNull()
        {
            var invalidId = 4;

            var result = await _storyProvider.GetItem(invalidId);
            Assert.That(result, Is.Null);
            _mockStoryService.Verify(x => x.GetItem(invalidId), Times.Once);
        }

        [Test]
        public async Task GetNewStoryIds_WithNoCache_ReturnsIdsFromService()
        {
            var result = await _storyProvider.GetNewStoryIds();
            Assert.That(result, Has.Exactly(3).Items);
            _mockStoryService.Verify(x => x.GetNewStories(), Times.Once);
        }

        [Test]
        public async Task GetNewStories_Allcached_ReturnsStoriesFromCache()
        {
            var cachedIds = new List<int> { 3 };
            var result = await _storyProvider.GetNewStories(cachedIds);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Exactly(1).Items);
            _mockStoryService.Verify(x => x.GetItems(It.IsAny<IEnumerable<int>>()), Times.Never);
        }

        [Test]
        public async Task GetNewStories_SomeCached_ReturnsStories()
        {
            var ids = new List<int> { 1, 2, 3 };

            var result = await _storyProvider.GetNewStories(ids);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Exactly(3).Items);
            _mockStoryService.Verify(x => x.GetItems(It.IsAny<IEnumerable<int>>()), Times.Once);
            _mockCache.Verify(x => x.AddStoriesToCache(It.IsAny<IEnumerable<Item>>()), Times.Once);
        }
    }
}