using System.Collections.Generic;
using TwitterFeed.Entities;

namespace TwitterFeed.Parsers
{
    public interface IUserParser
    {
        List<User> GetUsers(List<string> userLines);
    }
}