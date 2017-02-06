using System;
using System.Collections.Generic;
using System.Linq;
using TwitterFeed.Entities;

namespace TwitterFeed.Parsers
{
    public class TweetParser
    {
        public IEnumerable<Tweet> GetTweets(IEnumerable<string> tweetLines)
        {
            return tweetLines.Select(ParseTweet);
        }

        public Tweet ParseTweet(string tweetLine)
        {
            var parts = GetTweetParts(tweetLine);
            return new Tweet {Author = parts[0], Text = parts[1]};
        }

        private string[] GetTweetParts(string tweetLine)
        {
            var parts = tweetLine.Split(new[] {"> "}, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                throw new ArgumentException();
            }
            return parts;
        }
    }
}