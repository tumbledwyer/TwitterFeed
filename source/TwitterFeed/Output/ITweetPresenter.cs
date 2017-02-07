using TwitterFeed.Entities;

namespace TwitterFeed.Output
{
    public interface ITweetPresenter
    {
        void Render(User user);
        void Render(Tweet tweet);
    }
}
