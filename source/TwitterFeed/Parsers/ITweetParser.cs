using TwitterFeed.Entities;

namespace TwitterFeed.Parsers
{
    public interface ITweetParser
    {
        Tweet ParseTweet(string tweetLine);
    }
}