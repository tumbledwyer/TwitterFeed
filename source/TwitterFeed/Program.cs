using System;
using TwitterFeed.Output;
using TwitterFeed.Parsers;
using TwitterFeed.Readers;

namespace TwitterFeed
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var twitterApp = CreateTwitterApp();
                twitterApp.Run(args);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static TwitterApp CreateTwitterApp()
        {
            return new TwitterApp(new ConsoleTweetPresenter(), new TweetReader(new TweetParser()), new UserReader(new UserParser()));
        }
    }
}
