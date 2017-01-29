using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitterFeed.Output;

namespace TwitterFeed
{
    public class TwitterApp
    {
        private readonly ILogger _logger;

        public TwitterApp(ILogger logger)
        {
            _logger = logger;
        }

        public void Run(params string[] filePaths)
        {
            if (filePaths.Length != 2)
            {
                throw new ArgumentException("Method requires 2 file paths");
            }

            var userLines = File.ReadAllLines(filePaths[0]).OrderBy(s => s).ToList();

            var users = CreateUsers(userLines);

            var tweetLines = File.ReadAllLines(filePaths[1]);
            var tweets = tweetLines.Select(CreateTweet).ToList();
            
            users.ForEach(u =>
            {
                _logger.Log(u.Name);
                tweets.ForEach(t =>
                {
                    if (t.StartsWith($"\t@{u.Name}"))
                        _logger.Log(t);
                    u.Following.ForEach(f =>
                    {
                        if (t.StartsWith($"\t@{f.Name}"))
                            _logger.Log(t);
                    });
                });
            });
            
        }

        private List<User> CreateUsers(List<string> userLines)
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
            if (users.All(u => u.Name != user.Name))
            {
                users.Add(user);
            }
        }

        private User CreateNewUser(string primary)
        {
            return new User
            {
                Name = primary
            };
        }

        private string CreateTweet(string tweetLine)
        {
            var strings = tweetLine.Split(new [] {"> "}, StringSplitOptions.RemoveEmptyEntries);
            return $"\t@{strings[0]}: {strings[1]}";
        }
    }

    public class User
    {
        public User()
        {
            Following = new List<User>();
        }
        public string Name { get; set; }
        public List<User> Following { get; }
    }
}
