using System.Collections.Generic;
using MiniBook.Data.Entities.Base;
using MongoDB.Bson;

namespace MiniBook.Data.Entities
{
    public class Feed : ModelBase<ObjectId>
    {
        public string UserId { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}