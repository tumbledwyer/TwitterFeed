using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TwitterFeed.Parsers;

namespace TwitterFeed.Tests.Parsers
{
    [TestFixture]
    public class TweetParserTests
    {
        [Test]
        public void GetTweets_GivenZeroLines_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------
            var lines = new List<string> {  };
            var tweetParser = new TweetParser();
            //---------------Execute Test ----------------------
            var tweets = tweetParser.GetTweets(lines);
            //---------------Test Result -----------------------
            CollectionAssert.IsEmpty(tweets);
        }

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

        [Test]
        public void GetTweets_GivenWellFormedTweetLine_ShouldReturnTweet()
        {
            //---------------Set up test pack-------------------
            var lines = new List<string> { "Alex> Look at my horse" };
            var tweetParser = new TweetParser();
            //---------------Execute Test ----------------------
            var tweets = tweetParser.GetTweets(lines);
            //---------------Test Result -----------------------
            var tweet = tweets.First();
            Assert.AreEqual("Alex", tweet.Author);
            Assert.AreEqual("Look at my horse", tweet.Text);
        }

        [TestCase("")]
        [TestCase("Alex Look at my horse")]
        [TestCase("Alex> ")]
        [TestCase("> I'll take you to the universe")]
        [TestCase("Jim@ I'll take you to the universe")]
        public void GetTweets_GivenMalFormedTweetLine_ShouldThrowException(string line)
        {
            //---------------Set up test pack-------------------
            var tweetLine = line;
            var tweetParser = new TweetParser();
            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentException>(() => tweetParser.ParseTweet(tweetLine));
        }

        [Test]
        public void GetTweets_GivenMultipleWellFormedTweetLines_ShouldReturnTweets()
        {
            //---------------Set up test pack-------------------
            var lines = new List<string> { "Alex> Look at my horse", "Jim> My horse is amazing", "Alex> Sweet lemonade" };
            var tweetParser = new TweetParser();
            //---------------Execute Test ----------------------
            var tweets = tweetParser.GetTweets(lines).ToList();
            //---------------Test Result -----------------------
            Assert.AreEqual("Alex", tweets[0].Author);
            Assert.AreEqual("Look at my horse", tweets[0].Text);
            Assert.AreEqual("Jim", tweets[1].Author);
            Assert.AreEqual("My horse is amazing", tweets[1].Text);
            Assert.AreEqual("Alex", tweets[2].Author);
            Assert.AreEqual("Sweet lemonade", tweets[2].Text);
        }
    }
}
