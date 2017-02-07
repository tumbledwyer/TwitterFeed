using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.RandomGenerators;
using TwitterFeed.Entities;
using TwitterFeed.Output;
using TwitterFeed.Readers;
using TwitterFeed.Tests.TestUtils;

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
        public void Run_GivenMultipleTweetsAndFollowers_ShouldRenderUsersAlphabeticallyAndTweetsChronologically()
        {
            //---------------Set up test pack-------------------
            var userFile = FileUtils.GetTestFile("SampleUsers.txt");
            var tweetFile = FileUtils.GetTestFile("SampleTweets.txt");

            var jim = CreateUser("Jim");
            var zoe = CreateUser("Zoe");
            var bob = CreateUser("Bob");

            jim.Following.Add(zoe);
            zoe.Following.Add(bob);
            zoe.Following.Add(jim);

            var tweet1 = CreateTweet("Jim", RandomValueGen.GetRandomString(0, 140));
            var tweet2 = CreateTweet("Bob", RandomValueGen.GetRandomString(0, 140));
            var tweet3 = CreateTweet("Zoe", RandomValueGen.GetRandomString(0, 140));
            var tweet4 = CreateTweet("Zoe", RandomValueGen.GetRandomString(0, 140));

            var tweetReader = CreateTweetReader();
            tweetReader.ReadTweets(tweetFile).Returns(new List<Tweet> {tweet1, tweet2, tweet3, tweet4});

            var userReader = CreateUserReader();
            userReader.ReadUsers(userFile).Returns(new List<User> {jim, zoe, bob});
            
            var presenter = CreatePresenter();

            var twitterApp = CreateTwitterApp(presenter, tweetReader, userReader);
            //---------------Execute Test ----------------------
            twitterApp.Run(userFile, tweetFile);
            //---------------Test Result -----------------------
            Received.InOrder(() =>
            {
                presenter.Render(bob);
                presenter.Render(tweet2);
                presenter.Render(jim);
                presenter.Render(tweet1);
                presenter.Render(tweet3);
                presenter.Render(tweet4);
                presenter.Render(zoe);
                presenter.Render(tweet1);
                presenter.Render(tweet2);
                presenter.Render(tweet3);
                presenter.Render(tweet4);
            });
        }

        private TwitterApp CreateTwitterApp()
        {
            var presenter = CreatePresenter();
            var tweetReader = CreateTweetReader();
            var userReader = CreateUserReader();
            return CreateTwitterApp(presenter, tweetReader, userReader);
        }

        private TwitterApp CreateTwitterApp(ITweetPresenter tweetPresenter, ITweetReader tweetReader, IUserReader userReader)
        {
            return new TwitterApp(tweetPresenter, tweetReader, userReader);
        }

        private static IUserReader CreateUserReader()
        {
            return Substitute.For<IUserReader>();
        }

        private static ITweetReader CreateTweetReader()
        {
            return Substitute.For<ITweetReader>();
        }

        private ITweetPresenter CreatePresenter()
        {
            return Substitute.For<ITweetPresenter>();
        }

        private User CreateUser(string name)
        {
            return new User {Name = name};
        }

        private Tweet CreateTweet(string author, string text)
        {
            return new Tweet {Author = author, Text = text};
        }
    }
}
