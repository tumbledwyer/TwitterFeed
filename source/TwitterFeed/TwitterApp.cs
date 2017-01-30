using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitterFeed.Output;

namespace TwitterFeed
{
    public class TwitterApp
    {
        private readonly ITweetPresenter _tweetPresenter;
        private readonly UserParser _userParser;
        private readonly TweetParser _tweetParser;

        public TwitterApp(ITweetPresenter tweetPresenter)
        {
            _tweetPresenter = tweetPresenter;
            _userParser = new UserParser();
            _tweetParser = new TweetParser();
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
            var tweets = _tweetParser.GetTweets(tweetLines);

            var userTweets = users.OrderBy(user => user.Name)
                .Join(tweets, user => user.Name, tweet => tweet.Author, (user, tweet) => new {User = user, Tweet = tweet})
                .GroupBy(u => u.User, u => u.Tweet);

            foreach (var userTweet in userTweets)
            {
                _tweetPresenter.Render(userTweet.Key.Name);
                foreach (var tweet in userTweet)
                {
                    _tweetPresenter.Render(FormatTweet(tweet));
                }

            }

            //users.OrderBy(user => user.Name).ToList().ForEach(u =>
            //{
            //    _tweetPresenter.Render(u.Name);
            //    tweets.Where(ShouldShowTweet(u)).ToList()
            //        .ForEach(t => _tweetPresenter.Render(FormatTweet(t)));
            //});
        }

        private Func<Tweet, bool> ShouldShowTweet(User u)
        {
            return t =>TweetIsForUser(t, u) || u.Following.Any(f => TweetIsForUser(t, f));
        }

        private string FormatTweet(Tweet tweet)
        {
            return $"\t@{tweet.Author}: {tweet.Text}";
        }

        private bool TweetIsForUser(Tweet tweet, User user)
        {
            return tweet.Author == user.Name;
        }
    }
}
