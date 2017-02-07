using System;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.RandomGenerators;
using TwitterFeed.Entities;
using TwitterFeed.Parsers;
using TwitterFeed.Readers;
using TwitterFeed.Tests.TestUtils;

namespace TwitterFeed.Tests.Readers
{
    [TestFixture]
    public class TweetReaderTests
    {
        [Test]
        public void ReadTweets_GivenInvalidPath_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var filePath = "c:\\thisfileisntthere.dat";
            var tweetReader = CreateTweetReader();
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<Exception>(() => tweetReader.ReadTweets(filePath));
            //---------------Test Result -----------------------
            Assert.AreEqual("Invalid file name", exception.Message);
        }

        [Test]
        public void ReadTweets_GivenValidPath_ShouldReturnParsedTweets()
        {
            //---------------Set up test pack-------------------
            var filePath = FileUtils.GetTestFile("OneTweet.txt");

            var inputTweet = "Steve> I have a twitter account";
            var expectedTweet = CreateTweet();
            var tweetParser = CreateTweetParser(inputTweet, expectedTweet);

            var tweetReader = CreateTweetReader(tweetParser);
            //---------------Execute Test ----------------------
            var tweets = tweetReader.ReadTweets(filePath);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedTweet, tweets.First());
        }

        //[Test]
        //public void GetTweets_GivenMultipleWellFormedTweetLines_ShouldReturnTweets()
        //{
        //    //---------------Set up test pack-------------------
        //    var lines = new List<string> { "Alex> Look at my horse", "Jim> My horse is amazing", "Alex> Sweet lemonade" };
        //    var tweetParser = new TweetParser();
        //    //---------------Execute Test ----------------------
        //    var tweets = tweetParser.GetTweets(lines).ToList();
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual("Alex", tweets[0].Author);
        //    Assert.AreEqual("Look at my horse", tweets[0].Text);
        //    Assert.AreEqual("Jim", tweets[1].Author);
        //    Assert.AreEqual("My horse is amazing", tweets[1].Text);
        //    Assert.AreEqual("Alex", tweets[2].Author);
        //    Assert.AreEqual("Sweet lemonade", tweets[2].Text);
        //}

        private ITweetParser CreateTweetParser(string inputTweet, Tweet expectedTweet)
        {
            var tweetParser = Substitute.For<ITweetParser>();
            tweetParser.ParseTweet(inputTweet).Returns(expectedTweet);
            return tweetParser;
        }

        private Tweet CreateTweet()
        {
            return new Tweet
            {
                Author = RandomValueGen.GetRandomString(),
                Text = RandomValueGen.GetRandomString()
            };
        }

        private TweetReader CreateTweetReader(ITweetParser tweetParser = null)
        {
            tweetParser = tweetParser ?? Substitute.For<ITweetParser>();
            return new TweetReader(tweetParser);
        }
    }
}
