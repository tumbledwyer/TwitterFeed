using System;
using System.Collections.Generic;
using System.Linq;

namespace TwitterFeed
{
    public class UserParser
    {
        public List<User> GetUsers(List<string> userLines)
        {
            var users = new List<User>();
            userLines.ForEach(line =>
            {
                CreateUsers(line, users);
            });
            return users;
        }

        private void CreateUsers(string line, List<User> users)
        {
            var userNames = GetUserNames(line);
            var primary = userNames.First();
            var following = userNames.Skip(1).ToList();

            var user = users.FirstOrDefault(u => u.Name == primary) ?? CreateNewUser(primary);
            AddUser(users, user);

            following.ForEach(f =>
            {
                var followee = users.FirstOrDefault(u => u.Name == f) ?? CreateNewUser(f);
                AddUser(user.Following, followee);
                AddUser(users, followee);
            });
        }

        private IEnumerable<string> GetUserNames(string input)
        {
            return input.Split(new[] {"follows", ","}, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());
        }

        private void AddUser(List<User> users, User user)
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