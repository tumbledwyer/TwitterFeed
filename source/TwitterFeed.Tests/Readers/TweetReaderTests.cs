using System;
using NUnit.Framework;
using TwitterFeed.Readers;

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
            var tweetReader = new TweetReader();
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<Exception>(() => tweetReader.ReadTweets(filePath));
            //---------------Test Result -----------------------
            Assert.AreEqual("Invalid file name", exception.Message);
        }
    }
}
