using MongoDB.Bson;
using System.Collections.Generic;

namespace MiniBook.Data.Entities
{
    public class Feed : Base.Entity<ObjectId>
    {
        public string UserId { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}