using System;

namespace TwitterFeed.Output
{
    public class ConsoleTweetPresenter : ITweetPresenter
    {
        public void Render(string message)
        {
            Console.WriteLine(message);
        }
    }
}