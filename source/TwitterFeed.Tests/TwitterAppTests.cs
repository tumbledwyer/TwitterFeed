using System;
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
        public void Run_GivenOneUserAndZeroTweets_ShouldRenderName()
        {
            //---------------Set up test pack-------------------
            var userFile = GetTestFile("OneUser.txt");
            var tweetFile = GetTestFile("EmptyTweets.txt");
            var expected = "Steve";

            var presenter = CreatePresenter();

            var twitterApp = CreateTwitterApp(presenter);
            //---------------Execute Test ----------------------
            twitterApp.Run(userFile, tweetFile);
            //---------------Test Result -----------------------
            presenter.Received(1).Render(expected);
        }

        [Test]
        public void Run_GivenOneUserAndOneTweet_ShouldRenderNameAndTweet()
        {
            //---------------Set up test pack-------------------
            var userFile = GetTestFile("OneUser.txt");
            var tweetFile = GetTestFile("OneTweet.txt");
            var expectedUser = "Steve";
            var expectedTweet = "\t@Steve: I have a twitter account";

            var presenter = CreatePresenter();

            var twitterApp = CreateTwitterApp(presenter);
            //---------------Execute Test ----------------------
            twitterApp.Run(userFile, tweetFile);
            //---------------Test Result -----------------------
            Received.InOrder(() =>
            {
                presenter.Render(expectedUser);
                presenter.Render(expectedTweet);
            });
        }

        [Test]
        public void Run_GivenTwoUsersAndOneTweet_ShouldRenderUsersAlphabeticallyAndTweetWithCorrectUser()
        {
            //---------------Set up test pack-------------------
            var userFile = GetTestFile("TwoUsers.txt");
            var tweetFile = GetTestFile("OneTweet.txt");
            var expectedUser1 = "Steve";
            var expectedUser2 = "Xander";
            var expectedTweet = "\t@Steve: I have a twitter account";

            var presenter = CreatePresenter();

            var twitterApp = CreateTwitterApp(presenter);
            //---------------Execute Test ----------------------
            twitterApp.Run(userFile, tweetFile);
            //---------------Test Result -----------------------
            Received.InOrder(() =>
            {
                presenter.Render(expectedUser1);
                presenter.Render(expectedTweet);
                presenter.Render(expectedUser2);
            });
        }

        [Test]
        public void Run_GivenOneTweetAndAUserWithAFollower_ShouldRenderTweetForBothUsers()
        {
            //---------------Set up test pack-------------------
            var userFile = GetTestFile("TwoUsersWithFollower.txt");
            var tweetFile = GetTestFile("OneTweet.txt");
            var expectedUser1 = "Steve";
            var expectedUser2 = "Xander";
            var expectedTweet = "\t@Steve: I have a twitter account";

            var presenter = CreatePresenter();

            var twitterApp = CreateTwitterApp(presenter);
            //---------------Execute Test ----------------------
            twitterApp.Run(userFile, tweetFile);
            //---------------Test Result -----------------------
            Received.InOrder(() =>
            {
                presenter.Render(expectedUser1);
                presenter.Render(expectedTweet);
                presenter.Render(expectedUser2);
                presenter.Render(expectedTweet);
            });
        }

        [Test]
        public void Run_GivenMultipleTweetsAndFollowers_ShouldRenderAllInOrder()
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

            var presenter = CreatePresenter();

            var twitterApp = CreateTwitterApp(presenter);
            //---------------Execute Test ----------------------
            twitterApp.Run(userFile, tweetFile);
            //---------------Test Result -----------------------
            Received.InOrder(() =>
            {
                presenter.Render(expectedUser1);
                presenter.Render(expectedTweet1);
                presenter.Render(expectedTweet3);
                presenter.Render(expectedUser2);
                presenter.Render(expectedUser3);
                presenter.Render(expectedTweet1);
                presenter.Render(expectedTweet2);
                presenter.Render(expectedTweet3);
            });
        }

        private static TwitterApp CreateTwitterApp()
        {
            var logger = CreatePresenter();
            return CreateTwitterApp(logger);
        }

        private static TwitterApp CreateTwitterApp(ITweetPresenter tweetPresenter)
        {
            return new TwitterApp(tweetPresenter);
        }

        private static string GetTestFile(string testFile)
        {
            var testDirectory = TestContext.CurrentContext.TestDirectory;
            return Path.Combine(testDirectory, "TestData", testFile);
        }

        private static ITweetPresenter CreatePresenter()
        {
            return Substitute.For<ITweetPresenter>();
        }
    }
}
