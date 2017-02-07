using System;
using System.IO;
using NUnit.Framework;
using TwitterFeed.Entities;
using TwitterFeed.Output;

namespace TwitterFeed.Tests.Output
{
    [TestFixture]
    public class ConsoleTweetPresenterTests
    {
        [Test]
        public void Render_GivenUser_ShouldPrintUserName()
        {
            using (var writer = new StringWriter())
            {
                //---------------Set up test pack-------------------
                Console.SetOut(writer);

                var userName = "Steve";
                var expected = userName + "\r\n";
                var user = CreateUser(userName);

                var presenter = CreatePresenter();
                //---------------Execute Test ----------------------
                presenter.Render(user);
                //---------------Test Result -----------------------
                Assert.AreEqual(expected, writer.ToString());
            }
        }

        [Test]
        public void Render_GivenTweet_ShouldPrintCorrectlyFormatted()
        {
            using (var writer = new StringWriter())
            {
                //---------------Set up test pack-------------------
                Console.SetOut(writer);

                var author = "Jim";
                var text = "Is this like facebook?";
                var expected = $"\t@{author}: {text}\r\n";
                var tweet = CreateTweet(author, text);

                var presenter = CreatePresenter();
                //---------------Execute Test ----------------------
                presenter.Render(tweet);
                //---------------Test Result -----------------------
                Assert.AreEqual(expected, writer.ToString());
            }
        }

        private static ConsoleTweetPresenter CreatePresenter()
        {
            return new ConsoleTweetPresenter();
        }

        private static Tweet CreateTweet(string author, string text)
        {
            return new Tweet {Author = author, Text = text};
        }

        private static User CreateUser(string userName)
        {
            return new User {Name = userName};
        }
    }
}
