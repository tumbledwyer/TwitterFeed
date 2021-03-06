using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitterFeed.Entities;
using TwitterFeed.Parsers;

namespace TwitterFeed.Readers
{
    public class TweetReader : ITweetReader
    {
        private readonly ITweetParser _tweetParser;

        public TweetReader(ITweetParser tweetParser)
        {
            _tweetParser = tweetParser;
        }

        public IEnumerable<Tweet> ReadTweets(string filePath)
        {
            CheckFilePath(filePath);

            return File.ReadLines(filePath)
                .Select(_tweetParser.ParseTweet);
        }

        private static void CheckFilePath(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("Invalid file name");
            }
        }
    }
}