using System;
using TwitterFeed.Entities;

namespace TwitterFeed.Output
{
    public class ConsoleTweetPresenter : ITweetPresenter
    {
        public void Render(User user)
        {
            Console.WriteLine(user.Name);
        }

        public void Render(Tweet tweet)
        {
            Console.WriteLine(FormatTweet(tweet));
        }

        private string FormatTweet(Tweet tweet)
        {
            return $"\t@{tweet.Author}: {tweet.Text}";
        }
    }
}