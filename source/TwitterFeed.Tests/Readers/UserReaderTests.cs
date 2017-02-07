using System;
using System.Collections.Generic;
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
    public class UserReaderTests
    {
        [Test]
        public void ReadUsers_GivenInvalidPath_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var filePath = "c:\\thisfileisntthereeither.dat";
            var userReader = CreateUserReader();
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<Exception>(() => userReader.ReadUsers(filePath));
            //---------------Test Result -----------------------
            Assert.AreEqual("Invalid file name", exception.Message);
        }

        [Test]
        public void ReadUsers_GivenValidPath_ShouldReturnParsedUsers()
        {
            //---------------Set up test pack-------------------
            var filePath = FileUtils.GetTestFile("TwoUsers.txt");
            var expected = new List<User> { CreateUser(), CreateUser()};

            var userParser = CreateUserParser(expected);

            var userReader = CreateUserReader(userParser);
            //---------------Execute Test ----------------------
            var parsedUsers = userReader.ReadUsers(filePath);
            //---------------Test Result -----------------------
            CollectionAssert.AreEqual(expected, parsedUsers);
        }

        private IUserParser CreateUserParser(List<User> expected)
        {
            var userParser = Substitute.For<IUserParser>();
            userParser.GetUsers(Arg.Any<List<string>>()).Returns(expected);
            return userParser;
        }

        private User CreateUser()
        {
            return new User
            {
                Name = RandomValueGen.GetRandomString()
            };
        }

        private UserReader CreateUserReader(IUserParser userParser = null)
        {
            userParser = userParser ?? Substitute.For<IUserParser>();
            return new UserReader(userParser);
        }
    }
}