using System;

namespace TwitterFeed
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var twitterApp = new TwitterApp(new ConsoleLogger());
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
