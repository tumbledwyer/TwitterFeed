using System;
using System.Collections.Generic;
using System.Linq;
using TwitterFeed.Entities;
using TwitterFeed.Output;
using TwitterFeed.Readers;

namespace TwitterFeed.App
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
            CheckArguments(filePaths);

            var users = _userReader.ReadUsers(filePaths[0]);
            var tweets = _tweetReader.ReadTweets(filePaths[1]);

            RenderTweets(users, tweets);
        }

        private void RenderTweets(IEnumerable<User> users, IEnumerable<Tweet> tweets)
        {
            foreach (var user in users.OrderBy(user => user.Name))
            {
                _tweetPresenter.Render(user);
                foreach (var tweet in tweets.Where(ShouldShowTweet(user)))
                {
                    _tweetPresenter.Render(tweet);
                }
            }
        }

        private Func<Tweet, bool> ShouldShowTweet(User user)
        {
            return t =>TweetIsForUser(t, user) || user.Following.Any(f => TweetIsForUser(t, f));
        }

        private bool TweetIsForUser(Tweet tweet, User user)
        {
            return tweet.Author == user.Name;
        }

        private void CheckArguments(string[] filePaths)
        {
            if (filePaths.Length != 2)
            {
                throw new ArgumentException("Method requires 2 file paths");
            }
        }
    }
}
