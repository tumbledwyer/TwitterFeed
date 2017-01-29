using System;
using NUnit.Framework;

namespace TwitterFeed.Tests
{
    [TestFixture]
    public class TwitterAppTests
    {
        [Test]
        public void Run_GivenEmptyList_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var filePaths = new string[] {};
            var twitterApp = new TwitterApp();
            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentException>(() => twitterApp.Run(filePaths));
        }

        [Test]
        public void Run_GivenOneFilePath_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var filePaths = new[] {"c:\\input.txt"};
            var twitterApp = new TwitterApp();
            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentException>(() => twitterApp.Run(filePaths));
        }

        [Test]
        public void Run_GivenOneUserAndOneTweet_ShouldLogNameAndTweet()
        {
            //---------------Set up test pack-------------------
            var twitterApp = new TwitterApp();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.Fail("Test Not Yet Implemented");
        }
    }
}
