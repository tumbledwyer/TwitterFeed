using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TwitterFeed.Tests
{
    [TestFixture]
    public class UserParserTests
    {
        [Test]
        public void GetUsers_GivenOneUser_ShouldReturnOneUser()
        {
            //---------------Set up test pack-------------------
            var lines = new List<string> {"John follows"};
            var userParser = new UserParser();
            //---------------Execute Test ----------------------
            var users = userParser.GetUsers(lines);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, users.Count);
            Assert.AreEqual("John", users.First().Name);
        }

        [Test]
        public void GetUsers_GivenTwoUsers_ShouldReturnTwoUser()
        {
            //---------------Set up test pack-------------------
            var lines = new List<string> {"John follows", "Bill follows"};
            var userParser = new UserParser();
            //---------------Execute Test ----------------------
            var users = userParser.GetUsers(lines);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, users.Count);
            Assert.NotNull(users.FirstOrDefault(user => user.Name == "John"));
            Assert.NotNull(users.FirstOrDefault(user => user.Name == "Bill"));
        }

        [Test]
        public void GetUsers_GivenTwoUsersWithFollowers_ShouldReturnUserAndFollowers()
        {
            //---------------Set up test pack-------------------
            var lines = new List<string> {"John follows Bill", "Bill follows James"};
            var userParser = new UserParser();
            //---------------Execute Test ----------------------
            var users = userParser.GetUsers(lines);
            //---------------Test Result -----------------------
            var bill = users.FirstOrDefault(user => user.Name == "Bill");
            var john = users.FirstOrDefault(user => user.Name == "John");

            Assert.AreEqual(3, users.Count);
            Assert.AreEqual("James", bill.Following.First().Name);
            Assert.AreEqual("Bill", john.Following.First().Name);
        }

        [Test]
        public void GetUsers_GivenCommaSepartedFollowers_ShouldReturnUserAndFollowers()
        {
            //---------------Set up test pack-------------------
            var lines = new List<string> {"John follows Bill, Tim, Ed"};
            var userParser = new UserParser();
            //---------------Execute Test ----------------------
            var users = userParser.GetUsers(lines);
            //---------------Test Result -----------------------
            var john = users.FirstOrDefault(user => user.Name == "John");

            Assert.AreEqual(4, users.Count);
            CollectionAssert.AreEquivalent(new [] {"Bill", "Tim", "Ed"}, john.Following.Select(user => user.Name));
        }
    }
}
