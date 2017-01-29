using System;
using System.IO;
using System.Linq;
using TwitterFeed.Output;

namespace TwitterFeed
{
    public class TwitterApp
    {
        private readonly ILogger _logger;
        private readonly UserParser _userParser;

        public TwitterApp(ILogger logger)
        {
            _logger = logger;
            _userParser = new UserParser();
        }

        public void Run(params string[] filePaths)
        {
            if (filePaths.Length != 2)
            {
                throw new ArgumentException("Method requires 2 file paths");
            }

            var userLines = File.ReadLines(filePaths[0]).OrderBy(s => s).ToList();

            var users = _userParser.GetUsers(userLines);

            var tweetLines = File.ReadLines(filePaths[1]);
            var tweets = tweetLines.Select(CreateTweet).ToList();
            
            users.ForEach(u =>
            {
                _logger.Log(u.Name);
                tweets.ForEach(t =>
                {
                    if (t.StartsWith($"\t@{u.Name}"))
                        _logger.Log(t);
                    u.Following.ForEach(f =>
                    {
                        if (t.StartsWith($"\t@{f.Name}"))
                            _logger.Log(t);
                    });
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
