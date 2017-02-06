using System;
using System.Collections.Generic;
using System.IO;

namespace TwitterFeed
{
    public class TweetReader
    {
        private readonly TweetParser _tweetParser;

        public TweetReader()
        {
            _tweetParser = new TweetParser();
        }

        public IEnumerable<Tweet> ReadTweets(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("Invalid file name");
            }

            var tweetLines = File.ReadLines(filePath);
            return _tweetParser.GetTweets(tweetLines);
        }
    }
}