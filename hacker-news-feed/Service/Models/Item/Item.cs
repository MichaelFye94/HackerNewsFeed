using System.Collections.Generic;

namespace hacker_news_feed.Service.Models.Item
{
    public enum ItemType
    {
        None = 0,
        Comment = 1,
        Job = 2,
        Poll = 3,
        Pollopt = 4,
        Story = 5
    }

    public class Item
    {
        public string By { get; set; }
        public bool Dead { get; set; }
        public bool Deleted { get; set; }
        public int Descendants { get; set; }
        public int Id { get; set; }
        public List<int> Kids { get; set; }
        public int Parent { get; set; }
        public List<int> Parts { get; set; }
        public int Poll { get; set; }
        public int Score { get; set; }
        public string Text { get; set; }
        public long Time { get; set; }
        public string Title { get; set; }
        public ItemType Type { get; set; }
        public string Url { get; set; }
    }
}