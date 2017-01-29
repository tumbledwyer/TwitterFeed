using System;
using System.Collections.Generic;
using System.Linq;

namespace TwitterFeed
{
    public class TweetParser
    {
        public List<Tweet> GetTweets(IEnumerable<string> tweetLines)
        {
            return tweetLines.Select(CreateTweet).ToList();
        }

        private Tweet CreateTweet(string tweetLine)
        {
            var parts = tweetLine.Split(new [] {"> "}, StringSplitOptions.RemoveEmptyEntries);
            return new Tweet {Author = parts[0], Text = parts[1]};
        }
    }
}