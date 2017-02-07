using System;
using NUnit.Framework;
using TwitterFeed.Readers;

namespace TwitterFeed.Tests.Readers
{
    [TestFixture]
    public class UserReaderTests
    {
        [Test]
        public void ReadTweets_GivenInvalidPath_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var filePath = "c:\\thisfileisntthereeither.dat";
            var userReader = CreateUserReader();
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<Exception>(() => userReader.ReadUsers(filePath));
            //---------------Test Result -----------------------
            Assert.AreEqual("Invalid file name", exception.Message);
        }

        private UserReader CreateUserReader()
        {
            return new UserReader();
        }
    }
}