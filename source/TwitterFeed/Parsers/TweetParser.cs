using System;
using TwitterFeed.Entities;

namespace TwitterFeed.Parsers
{
    public class TweetParser : ITweetParser
    {
        public Tweet ParseTweet(string tweetLine)
        {
            var parts = GetTweetParts(tweetLine);
            return CreateTweet(parts);
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

        private Tweet CreateTweet(string[] parts)
        {
            return new Tweet
            {
                Author = parts[0],
                Text = TruncateText(parts[1])
            };
        }

        private string TruncateText(string text)
        {
            return text.Length > 140
                ? text.Substring(0, 140)
                : text;
        }
    }
}