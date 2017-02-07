using System;
using System.Linq;
using TwitterFeed.Entities;
using TwitterFeed.Output;
using TwitterFeed.Readers;

namespace TwitterFeed
{
    public class TwitterApp
    {
        private readonly ITweetPresenter _tweetPresenter;
        private readonly IUserReader _userReader;
        private readonly ITweetReader _tweetReader;

        public TwitterApp(ITweetPresenter tweetPresenter, ITweetReader tweetReader, IUserReader userReader)
        {
            _tweetPresenter = tweetPresenter;
            _tweetReader = tweetReader;
            _userReader = userReader;
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
                _tweetPresenter.Render(u);
                tweets.Where(ShouldShowTweet(u)).ToList()
                    .ForEach(t => _tweetPresenter.Render(t));
            });
        }

        private Func<Tweet, bool> ShouldShowTweet(User user)
        {
            return t =>TweetIsForUser(t, user) || user.Following.Any(f => TweetIsForUser(t, f));
        }

        private bool TweetIsForUser(Tweet tweet, User user)
        {
            return tweet.Author == user.Name;
        }
    }
}
