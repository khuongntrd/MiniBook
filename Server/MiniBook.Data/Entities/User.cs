using System.Collections.Generic;

namespace MiniBook.Data.Entities
{
    public class User:Base.ModelBase<string>
    {
        public Profile Profile { get; set; }

        public Dictionary<string, Friend> Followers { get; set; } = new Dictionary<string, Friend>();
    }
}