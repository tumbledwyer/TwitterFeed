using System;
using NUnit.Framework;
using PeanutButter.RandomGenerators;
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
            var tweetParser = CreateTweetParser();
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
            var tweetParser = CreateTweetParser();
            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentException>(() => tweetParser.ParseTweet(tweetLine));
        }

        [Test]
        public void ParseTweet_GivenTweetOver140Chars_ShouldReturnTruncatedTweet()
        {
            //---------------Set up test pack-------------------
            var tweet160Chars = RandomValueGen.GetRandomString(160, 160);
            var expectedTweet = tweet160Chars.Substring(0, 140);
            var tweetLine = $"Alex> {tweet160Chars}";

            var tweetParser = CreateTweetParser();
            //---------------Execute Test ----------------------
            var tweet = tweetParser.ParseTweet(tweetLine);
            //---------------Test Result -----------------------
            Assert.AreEqual("Alex", tweet.Author);
            Assert.AreEqual(expectedTweet, tweet.Text);
        }

        private static TweetParser CreateTweetParser()
        {
            return new TweetParser();
        }
    }
}
