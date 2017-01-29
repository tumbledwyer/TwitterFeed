using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            var readAllLines = File.ReadAllLines(filePaths[0]).ToList();
            var users = new List<string>();
            readAllLines.ForEach(s => users.AddRange(s.Split(" follows".ToArray(), StringSplitOptions.RemoveEmptyEntries)));
            users.ForEach(s => _logger.Log(s));
        }
    }
}
