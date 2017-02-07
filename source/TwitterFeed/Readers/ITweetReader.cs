using System.Collections.Generic;
using TwitterFeed.Entities;

namespace TwitterFeed.Readers
{
    public interface ITweetReader
    {
        IEnumerable<Tweet> ReadTweets(string filePath);
    }
}