using System;
using System.Collections.Generic;

namespace snipetrain_bot.Models
{
    public class TwitchUserWrapper
    {
        public List<TwitchUser> Data { get; set; }
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
    }
}