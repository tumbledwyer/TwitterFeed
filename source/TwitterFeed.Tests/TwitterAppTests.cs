using System;
using System.IO;
using NSubstitute;
using NUnit.Framework;
using TwitterFeed.Output;

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
            var twitterApp = CreateTwitterApp();
            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentException>(() => twitterApp.Run(filePaths));
        }

        [Test]
        public void Run_GivenOneFilePath_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var filePaths = new[] {"c:\\input.txt"};
            var twitterApp = CreateTwitterApp();
            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentException>(() => twitterApp.Run(filePaths));
        }

        [Test]
        public void Run_GivenOneUserAndZeroTweets_ShouldLogName()
        {
            //---------------Set up test pack-------------------
            var userFile = GetTestFile("OneUser.txt");
            var tweetFile = GetTestFile("EmptyTweets.txt");
            var expected = "Steve";

            var logger = CreateLogger();

            var twitterApp = CreateTwitterApp(logger);
            //---------------Execute Test ----------------------
            twitterApp.Run(userFile, tweetFile);
            //---------------Test Result -----------------------
            logger.Received(1).Log(expected);
        }

        [Test]
        public void Run_GivenOneUserAndOneTweet_ShouldLogNameAndTweet()
        {
            //---------------Set up test pack-------------------
            var userFile = GetTestFile("OneUser.txt");
            var tweetFile = GetTestFile("OneTweet.txt");
            var expectedUser = "Steve";
            var expectedTweet = "\t@Steve: I have a twitter account";

            var logger = CreateLogger();

            var twitterApp = CreateTwitterApp(logger);
            //---------------Execute Test ----------------------
            twitterApp.Run(userFile, tweetFile);
            //---------------Test Result -----------------------
            Received.InOrder(() =>
            {
                logger.Log(expectedUser);
                logger.Log(expectedTweet);
            });
        }

        private static TwitterApp CreateTwitterApp()
        {
            var logger = CreateLogger();
            return CreateTwitterApp(logger);
        }

        private static TwitterApp CreateTwitterApp(ILogger logger)
        {
            return new TwitterApp(logger);
        }

        private static string GetTestFile(string testFile)
        {
            var testDirectory = TestContext.CurrentContext.TestDirectory;
            return Path.Combine(testDirectory, "TestData", testFile);
        }

        private static ILogger CreateLogger()
        {
            return Substitute.For<ILogger>();
        }
    }
}
