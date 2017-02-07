using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitterFeed.Entities;
using TwitterFeed.Parsers;

namespace TwitterFeed.Readers
{
    public class UserReader
    {
        private readonly IUserParser _userParser;

        public UserReader(IUserParser userParser)
        {
            _userParser = userParser;
        }

        public IEnumerable<User> ReadUsers(string filePath)
        {
            CheckFilePath(filePath);

            var userLines = File.ReadLines(filePath).ToList();
            return _userParser.GetUsers(userLines);
        }

        private static void CheckFilePath(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("Invalid file name");
            }
        }
    }
}