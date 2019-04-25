using MiniBook.Resource.Models;
using MongoDB.Driver;

namespace MiniBook.Resource.Data
{
    public class DataContext
    {
        private MongoClient MongoClient { get; }
        private IMongoDatabase Database { get; }

        public DataContext(string connectionString, string dbName)
        {
            MongoClient = new MongoClient(connectionString);

            Database = MongoClient.GetDatabase(dbName);
        }

        public IMongoCollection<Post> Posts => Database.GetCollection<Post>("posts");
    }
}
