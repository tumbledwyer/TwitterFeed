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

            var userLines = File.ReadLines(filePaths[0]).ToList();

            var users = _userParser.GetUsers(userLines);

            var tweetLines = File.ReadLines(filePaths[1]);
            var tweets = tweetLines.Select(CreateTweet).ToList();
            
            users.OrderBy(user => user.Name).ToList()
                .ForEach(u =>
            {
                _logger.Log(u.Name);
                tweets.Where(ShouldShowTweet(u)).ToList()
                    .ForEach(t => _logger.Log(FormatTweet(t)));
            });
        }

        private static Func<Tweet, bool> ShouldShowTweet(User u)
        {
            return t =>TweetIsForUser(t, u) || u.Following.Any(f => TweetIsForUser(t, f));
        }

        private string FormatTweet(Tweet tweet)
        {
            return $"\t@{tweet.Author}: {tweet.Text}";
        }

        private static bool TweetIsForUser(Tweet tweet, User user)
        {
            return tweet.Author == user.Name;
        }

        private Tweet CreateTweet(string tweetLine)
        {
            var parts = tweetLine.Split(new [] {"> "}, StringSplitOptions.RemoveEmptyEntries);
            return new Tweet {Author = parts[0], Text = parts[1]};
        }
    }

    public class Tweet
    {
        public string Author { get; set; }
        public string Text { get; set; }
    }
}
