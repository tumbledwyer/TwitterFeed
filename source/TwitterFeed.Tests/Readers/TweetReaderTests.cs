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
