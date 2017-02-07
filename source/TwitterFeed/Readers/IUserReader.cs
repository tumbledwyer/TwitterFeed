using System.Collections.Generic;
using TwitterFeed.Entities;

namespace TwitterFeed.Readers
{
    public interface IUserReader
    {
        IEnumerable<User> ReadUsers(string filePath);
    }
}