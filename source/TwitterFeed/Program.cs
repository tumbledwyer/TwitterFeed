using System;
using TwitterFeed.Output;

namespace TwitterFeed
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var twitterApp = new TwitterApp(new ConsoleTweetPresenter());
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
    }
}
