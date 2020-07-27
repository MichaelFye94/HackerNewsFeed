using hacker_news_feed.Service.Interfaces.Story;
using hacker_news_feed.Service.Models.Item;
using hacker_news_feed.Service.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hacker_news_feed.Tests.Service.Services
{
    [TestFixture]
    class StoryServiceTests
    {
        private IStoryService _storyService;

        [SetUp]
        public void SetUp()
        {
            var mock = new Mock<IStoryData>();

            IEnumerable<int> newStories = new List<int> { 1, 2, 3 };
            mock.Setup(x => x.GetNewStories())
                .Returns(Task.FromResult(newStories));

            var story = new Item
            {
                Id = 1,
                Title = "Test",
                Type = ItemType.Story,
                Url = "https://www.google.com"
            };
            var story2 = new Item
            {
                Id = 2,
                Title = "Test",
                Type = ItemType.Story,
                Url = "https://www.yahoo.com"
            };
            mock.Setup(x => x.GetStory(1))
                .Returns(Task.FromResult(story));
            mock.Setup(x => x.GetStory(2))
                .Returns(Task.FromResult(story2));
            mock.Setup(x => x.GetStory(It.IsNotIn(new int[] { 1, 2 })))
                .Returns(Task.FromResult<Item>(null));

            _storyService = new StoryService(mock.Object);
        }

        [Test]
        public async Task GetNewStories_ReturnsIdList()
        {
            var expectedList = new List<int> { 1, 2, 3 };

            var result = await _storyService.GetNewStories();
            Assert.That(result, Has.Exactly(3).Items);
            Assert.That(result, Is.EqualTo(expectedList));
        }

        [Test]
        public async Task GetItem_ValidId_ReturnsItem()
        {
            var expectedStory = new Item
            {
                Id = 1,
                Title = "Test",
                Type = ItemType.Story,
                Url = "https://www.google.com"
            };

            var result = await _storyService.GetItem(expectedStory.Id);
            Assert.That(result.Id, Is.EqualTo(expectedStory.Id));
            Assert.That(result.Title, Is.EqualTo(expectedStory.Title));
            Assert.That(result.Type, Is.EqualTo(expectedStory.Type));
            Assert.That(result.Url, Is.EqualTo(expectedStory.Url));
        }

        [Test]
        public async Task GetItem_InvalidId_ReturnsNull()
        {
            var invalidId = 3;
            var result = await _storyService.GetItem(invalidId);
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetItems_AllValidIds_ReturnsAllStories()
        {
            var result = await _storyService.GetItems(new List<int> { 1, 2 });
            Assert.That(result, Has.Exactly(2).Items);
        }

        [Test]
        public async Task GetItems_SomeValidIds_ReturnsOnlyValidIds()
        {
            var result = await _storyService.GetItems(new List<int> { 1, 2, 3 });
            Assert.That(result, Has.Exactly(2).Items);
            Assert.That(result, Has.Exactly(0).Items.EqualTo(3));
        }
    }
}
