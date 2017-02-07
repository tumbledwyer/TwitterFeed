using System;
using System.IO;
using NUnit.Framework;
using TwitterFeed.Output;
using TwitterFeed.Parsers;
using TwitterFeed.Readers;
using TwitterFeed.Tests.TestUtils;

namespace TwitterFeed.Tests
{
    public class TwitterAppIntegrationTests
    {
       [Category("Integration")]
        [Test]
        public void Run_GivenOneUserAndZeroTweets_ShouldRenderName()
        {
            using (var writer = new StringWriter())
            {
                //---------------Set up test pack-------------------
                Console.SetOut(writer);
                var userFile = FileUtils.GetTestFile("OneUser.txt");
                var tweetFile = FileUtils.GetTestFile("EmptyTweets.txt");
                var expectedOutput = "Steve\r\n";

                var twitterApp = CreateTwitterApp();
                //---------------Execute Test ----------------------
                twitterApp.Run(userFile, tweetFile);
                //---------------Test Result -----------------------
                Assert.AreEqual(expectedOutput, writer.ToString());
            }
        }

        [Category("Integration")]
        [Test]
        public void Run_GivenOneUserAndOneTweet_ShouldRenderNameAndTweet()
        {
            using (var writer = new StringWriter())
            {
                //---------------Set up test pack-------------------
                Console.SetOut(writer);
                var userFile = FileUtils.GetTestFile("OneUser.txt");
                var tweetFile = FileUtils.GetTestFile("OneTweet.txt");
                var expectedOutput = "Steve\r\n" +
                                     "\t@Steve: I have a twitter account\r\n";

                var twitterApp = CreateTwitterApp();
                //---------------Execute Test ----------------------
                twitterApp.Run(userFile, tweetFile);
                //---------------Test Result -----------------------
                Assert.AreEqual(expectedOutput, writer.ToString());
            }
        }

        [Category("Integration")]
        [Test]
        public void Run_GivenTwoUsersAndOneTweet_ShouldRenderUsersAlphabeticallyAndTweetWithCorrectUser()
        {
            using (var writer = new StringWriter())
            {
                //---------------Set up test pack-------------------
                Console.SetOut(writer);
                var userFile = FileUtils.GetTestFile("TwoUsers.txt");
                var tweetFile = FileUtils.GetTestFile("OneTweet.txt");
                var expectedOutput = "Steve\r\n" +
                                     "\t@Steve: I have a twitter account\r\n" +
                                     "Xander\r\n";

                var twitterApp = CreateTwitterApp();
                //---------------Execute Test ----------------------
                twitterApp.Run(userFile, tweetFile);
                //---------------Test Result -----------------------
                Assert.AreEqual(expectedOutput, writer.ToString());
            }
        }

        [Category("Integration")]
        [Test]
        public void Run_GivenOneTweetAndAUserWithAFollower_ShouldRenderTweetForBothUsers()
        {
            using (var writer = new StringWriter())
            {
                //---------------Set up test pack-------------------
                Console.SetOut(writer);
                var userFile = FileUtils.GetTestFile("TwoUsersWithFollower.txt");
                var tweetFile = FileUtils.GetTestFile("OneTweet.txt");
                var expectedOutput = "Steve\r\n" +
                                     "\t@Steve: I have a twitter account\r\n" +
                                     "Xander\r\n" +
                                     "\t@Steve: I have a twitter account\r\n";

                var twitterApp = CreateTwitterApp();
                //---------------Execute Test ----------------------
                twitterApp.Run(userFile, tweetFile);
                //---------------Test Result -----------------------
                Assert.AreEqual(expectedOutput, writer.ToString());
            }
        }

        [Category("Integration")]
        [Test]
        public void Run_GivenMultipleTweetsAndFollowers_ShouldRenderAllInOrder()
        {
            using (var writer = new StringWriter())
            {
                //---------------Set up test pack-------------------
                Console.SetOut(writer);
                var userFile = FileUtils.GetTestFile("SampleUsers.txt");
                var tweetFile = FileUtils.GetTestFile("SampleTweets.txt");
                var expectedOutput = "Alan\r\n" +
                                     "\t@Alan: If you have a procedure with 10 parameters, you probably missed some.\r\n" +
                                     "\t@Alan: Random numbers should not be generated with a method chosen at random.\r\n" +
                                     "Martin\r\n" +
                                     "Ward\r\n" +
                                     "\t@Alan: If you have a procedure with 10 parameters, you probably missed some.\r\n" +
                                     "\t@Ward: There are only two hard things in Computer Science: cache invalidation, naming things and off-by-1 errors.\r\n" +
                                     "\t@Alan: Random numbers should not be generated with a method chosen at random.\r\n";

                var twitterApp = CreateTwitterApp();
                //---------------Execute Test ----------------------
                twitterApp.Run(userFile, tweetFile);
                //---------------Test Result -----------------------
                Assert.AreEqual(expectedOutput, writer.ToString());
            }
        }

        private TwitterApp CreateTwitterApp()
        {
            return new TwitterApp(new ConsoleTweetPresenter(), new TweetReader(new TweetParser()), new UserReader(new UserParser()));
        }
    }
}