﻿using System;
using System.Collections.Generic;
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

        [Test]
        public void Run_GivenTwoUsersAndOneTweet_ShouldLogUsersAlphabeticallyAndTweetWithCorrectUser()
        {
            //---------------Set up test pack-------------------
            var userFile = GetTestFile("TwoUsers.txt");
            var tweetFile = GetTestFile("OneTweet.txt");
            var expectedUser1 = "Steve";
            var expectedUser2 = "Xander";
            var expectedTweet = "\t@Steve: I have a twitter account";

            var logger = CreateLogger();

            var twitterApp = CreateTwitterApp(logger);
            //---------------Execute Test ----------------------
            twitterApp.Run(userFile, tweetFile);
            //---------------Test Result -----------------------
            Received.InOrder(() =>
            {
                logger.Log(expectedUser1);
                logger.Log(expectedTweet);
                logger.Log(expectedUser2);
            });
        }

        [Test]
        public void Run_GivenOneTweetAndAUserWithAFollower_ShouldLogTweetForBothUsers()
        {
            //---------------Set up test pack-------------------
            var userFile = GetTestFile("TwoUsersWithFollower.txt");
            var tweetFile = GetTestFile("OneTweet.txt");
            var expectedUser1 = "Steve";
            var expectedUser2 = "Xander";
            var expectedTweet = "\t@Steve: I have a twitter account";

            var logger = CreateLogger();

            var twitterApp = CreateTwitterApp(logger);
            //---------------Execute Test ----------------------
            twitterApp.Run(userFile, tweetFile);
            //---------------Test Result -----------------------
            Received.InOrder(() =>
            {
                logger.Log(expectedUser1);
                logger.Log(expectedTweet);
                logger.Log(expectedUser2);
                logger.Log(expectedTweet);
            });
        }

        [Test]
        public void Run_GivenMultipleTweetsAndFollowers_ShouldAllInOrder()
        {
            //---------------Set up test pack-------------------
            var userFile = GetTestFile("SampleUsers.txt");
            var tweetFile = GetTestFile("SampleTweets.txt");
            var expectedUser1 = "Alan";
            var expectedUser2 = "Martin";
            var expectedUser3 = "Ward";
            var expectedTweet1 = "\t@Alan: If you have a procedure with 10 parameters, you probably missed some.";
            var expectedTweet2 = "\t@Ward: There are only two hard things in Computer Science: cache invalidation, naming things and off-by-1 errors.";
            var expectedTweet3 = "\t@Alan: Random numbers should not be generated with a method chosen at random.";

            var logger = CreateLogger();

            var twitterApp = CreateTwitterApp(logger);
            //---------------Execute Test ----------------------
            twitterApp.Run(userFile, tweetFile);
            //---------------Test Result -----------------------
            Received.InOrder(() =>
            {
                logger.Log(expectedUser1);
                logger.Log(expectedTweet1);
                logger.Log(expectedTweet3);
                logger.Log(expectedUser2);
                logger.Log(expectedUser3);
                logger.Log(expectedTweet1);
                logger.Log(expectedTweet2);
                logger.Log(expectedTweet3);
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
