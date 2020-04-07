using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MiniBook.Data.Entities
{
    public class Post : Base.Entity<ObjectId>
    {
        public Owner By { get; set; }
        [BsonRepresentation(BsonType.String)]
        public PostType Type { get; set; }
        public Detail Detail { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}