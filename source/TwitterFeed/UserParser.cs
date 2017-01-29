using System;
using System.Collections.Generic;
using System.Linq;

namespace TwitterFeed
{
    public class UserParser
    {
        public List<User> CreateUsers(List<string> userLines)
        {
            var users = new List<User>();
            userLines.ForEach(s =>
            {
                var userNames = s.Split(new[] {" follows"}, StringSplitOptions.RemoveEmptyEntries);
                var primary = userNames[0];
                var followers = userNames.Skip(1).ToList();
                var user = users.FirstOrDefault(u => u.Name == primary) ?? CreateNewUser(primary);
                followers.ForEach(f =>
                {
                    var followee = users.FirstOrDefault(u => u.Name == f.Trim()) ?? CreateNewUser(f);
                    AddUser(user.Following, followee);
                    AddUser(users, followee);
                });
                AddUser(users, user);
            });
            return users;
        }

        private static void AddUser(List<User> users, User user)
        {
            if (users.Any(u => u.Name == user.Name)) return;
            users.Add(user);
        }

        private User CreateNewUser(string primary)
        {
            return new User
            {
                Name = primary
            };
        }
    }
}