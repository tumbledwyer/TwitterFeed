using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitterFeed.Output;

namespace TwitterFeed
{
    public class TwitterApp
    {
        private readonly ILogger _logger;

        public TwitterApp(ILogger logger)
        {
            _logger = logger;
        }

        public void Run(params string[] filePaths)
        {
            if (filePaths.Length != 2)
            {
                throw new ArgumentException("Method requires 2 file paths");
            }

            var users = new List<string>();
            var userLines = File.ReadAllLines(filePaths[0]).OrderBy(s => s).ToList();
            userLines.ForEach(s => users.AddRange(s.Split(new [] {" follows"}, StringSplitOptions.RemoveEmptyEntries)));

            var tweetLines = File.ReadAllLines(filePaths[1]);
            var tweets = tweetLines.Select(CreateTweet).ToList();
            
            users.ForEach(s =>
            {
                _logger.Log(s);
                tweets.ForEach(t =>
                {
                    if (t.StartsWith($"\t@{s}"))
                        _logger.Log(t);
                });
            });
            
        }

        private string CreateTweet(string tweetLine)
        {
            var strings = tweetLine.Split(new [] {"> "}, StringSplitOptions.RemoveEmptyEntries);
            return $"\t@{strings[0]}: {strings[1]}";
        }
    }
}
