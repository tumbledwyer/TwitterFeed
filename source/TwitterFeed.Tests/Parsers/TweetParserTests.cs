using System;
using NUnit.Framework;
using TwitterFeed.Parsers;

namespace TwitterFeed.Tests.Parsers
{
    [TestFixture]
    public class TweetParserTests
    {
        
        [Test]
        public void ParseTweet_GivenWellFormedTweetLine_ShouldReturnTweet()
        {
            //---------------Set up test pack-------------------
            var lines = "Alex> Look at my horse";
            var tweetParser = new TweetParser();
            //---------------Execute Test ----------------------
            var tweet = tweetParser.ParseTweet(lines);
            //---------------Test Result -----------------------
            Assert.AreEqual("Alex", tweet.Author);
            Assert.AreEqual("Look at my horse", tweet.Text);
        }
        
        [TestCase("")]
        [TestCase("Alex Look at my horse")]
        [TestCase("Alex> ")]
        [TestCase("> I'll take you to the universe")]
        [TestCase("Jim@ And all the other places")]
        public void GetTweets_GivenMalFormedTweetLine_ShouldThrowException(string line)
        {
            //---------------Set up test pack-------------------
            var tweetLine = line;
            var tweetParser = new TweetParser();
            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentException>(() => tweetParser.ParseTweet(tweetLine));
        }
    }
}
