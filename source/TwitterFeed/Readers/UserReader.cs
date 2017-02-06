using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitterFeed.Entities;
using TwitterFeed.Parsers;

namespace TwitterFeed.Readers
{
    public class UserReader
    {
        private readonly UserParser _userParser;

        public UserReader()
        {
            _userParser = new UserParser();
        }

        public IEnumerable<User> ReadUsers(string filePath)
        {
            var userLines = File.ReadLines(filePath).ToList();
            return _userParser.GetUsers(userLines);
        }
    }
}