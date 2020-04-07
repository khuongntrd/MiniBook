using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniBook.Data.Context;
using MiniBook.Data.Entities;
using MiniBook.Data.Repositories.Base;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MiniBook.Data.Repositories
{
    public class FeedRepository : BaseRepository
    {
        public FeedRepository(ResourceDbContext context) : base(context)
        {

        }

        const int PAGE_SIZE = 2;

        public Task<List<Post>> GetNewsFeed(string userId, int page = 0)
        {
            return GetFeed(Context.News, userId, page * PAGE_SIZE, PAGE_SIZE);
        }

        public Task<List<Post>> GetWallFeed(string userId, int page = 0)
        {
            return GetFeed(Context.Wall, userId, page * PAGE_SIZE, PAGE_SIZE);
        }

        async Task<List<Post>> GetFeed(IMongoCollection<Feed> collection, string userId, int skip, int limit)
        {
            PipelineDefinition<Feed, Feed> pipeline = new[]
            {
                new BsonDocument {{"$match", new BsonDocument {{nameof(Feed.UserId), userId}}}},
                new BsonDocument {{"$unwind", "$Posts"}},
                new BsonDocument {{"$sort", new BsonDocument {{"Posts.Meta.Created", -1}}}},
                new BsonDocument {{"$skip", skip}},
                new BsonDocument {{"$limit", limit}},
                new BsonDocument {{"$group", new BsonDocument {{"_id", BsonNull.Value}, {"Posts", new BsonDocument("$push", "$Posts")}}}},
                new BsonDocument {{"$project", new BsonDocument {{"_id", 0}, {"Posts", 1}}}}
            };

            return (await collection.AggregateAsync(pipeline)).FirstOrDefault().Posts;
        }

        public async Task AppendPostAsync(Post post)
        {
            var wallDest = new List<string> { post.By.Id };
            var newsDest = new List<string> { post.By.Id };

            var followers = await Context.Users.Find(x => x.Id == post.By.Id).Project(x => x.Followers).ToListAsync();
            if (followers.All(x => x.Any()))
                newsDest.AddRange(followers.SelectMany(x => x.Keys));

            await Task.WhenAll(AppendPostAsync(Context.Wall, wallDest, post),
                AppendPostAsync(Context.News, newsDest, post));
        }

        public async Task<bool> AppendPostAsync(IMongoCollection<Feed> collection, List<string> destUsers, Post post)
        {
            var filter = Builders<Feed>.Filter.In(nameof(Feed.UserId), destUsers);
            var update = Builders<Feed>.Update.Push(nameof(Feed.Posts), post);
            var result = await collection.UpdateManyAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task AppendCommentAsync(string postId, Comment comment)
        {
            await Task.WhenAll(
                Context.Wall.UpdateManyAsync(
                    Builders<Feed>.Filter.Eq($"{nameof(Feed.Posts)}._id", ObjectId.Parse(postId)),
                    Builders<Feed>.Update.Push($"{nameof(Feed.Posts)}.$.{nameof(Post.Comments)}", comment)),
                Context.News.UpdateManyAsync(
                    Builders<Feed>.Filter.Eq($"{nameof(Feed.Posts)}._id", ObjectId.Parse(postId)),
                    Builders<Feed>.Update.Push($"{nameof(Feed.Posts)}.$.{nameof(Post.Comments)}", comment))
            );
        }


    }
}