using System;

namespace snipetrain_bot.Models
{
    public class TwitchWrapper<T>
    {
        public T[] Data { get; set; }
    }

    public class TwitchUser
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string DisplayName { get; set; }
        public string Type { get; set; }
        public string BroadcasterType { get; set; }
        public string Description { get; set; }
        public Uri ProfileImageUrl { get; set; }
        public Uri OfflineImageUrl { get; set; }
        public long ViewCount { get; set; }
        public string Email { get; set; }
    }
}