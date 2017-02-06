using System;
using System.Linq;
using TwitterFeed.Entities;
using TwitterFeed.Output;
using TwitterFeed.Parsers;
using TwitterFeed.Readers;

namespace TwitterFeed
{
    public class TwitterApp
    {
        private readonly ITweetPresenter _tweetPresenter;
        private readonly UserReader _userReader;
        private readonly TweetReader _tweetReader;

        public TwitterApp(ITweetPresenter tweetPresenter)
        {
            _tweetPresenter = tweetPresenter;
            _userReader = new UserReader();
            _tweetReader = new TweetReader(new TweetParser());
        }

        public void Run(params string[] filePaths)
        {
            if (filePaths.Length != 2)
            {
                throw new ArgumentException("Method requires 2 file paths");
            }

            var users = _userReader.ReadUsers(filePaths[0]);
            var tweets = _tweetReader.ReadTweets(filePaths[1]);

            users.OrderBy(user => user.Name).ToList().ForEach(u =>
            {
                _tweetPresenter.Render(u.Name);
                tweets.Where(ShouldShowTweet(u)).ToList()
                    .ForEach(t => _tweetPresenter.Render(FormatTweet(t)));
            });
        }

        private Func<Tweet, bool> ShouldShowTweet(User user)
        {
            return t =>TweetIsForUser(t, user) || user.Following.Any(f => TweetIsForUser(t, f));
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
