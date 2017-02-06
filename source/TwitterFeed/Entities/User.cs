using System.Collections.Generic;

namespace TwitterFeed.Entities
{
    public class User
    {
        public User()
        {
            Following = new List<User>();
        }
        public string Name { get; set; }
        public List<User> Following { get; }
    }
}