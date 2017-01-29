using System;

namespace TwitterFeed
{
    public class TwitterApp
    {
        public void Run(string[] filePaths)
        {
            if (filePaths.Length != 2)
            {
                throw new ArgumentException("Method requires 2 file paths");
            }
        }
    }
}
