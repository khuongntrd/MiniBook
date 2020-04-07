using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MiniBook.Data.Entities.Base
{
    public class ModelBase<T>
    {
        [BsonId]
        public T Id { get; set; }

        public Meta Meta { get; set; }

        public ModelBase()
        {
            Meta = new Meta();
        }
    }
    public class Meta
    {
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime? Deleted { get; set; }

        public Meta()
        {
            Updated = Created = DateTime.UtcNow;
        }
    }
}
