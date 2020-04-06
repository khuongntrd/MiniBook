using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniBook.Data.Context;
using MiniBook.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using Bson = MongoDB.Bson.BsonDocument;

namespace MiniBook.Data.Repositories
{
    public class FeedRepository
    {
        const int PAGE_SIZE = 10;

        public FeedRepository(ResourceDbContext context)
        {
            Context = context;
        }

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
                new Bson {{"$match", new Bson {{nameof(Feed.UserId), userId}}}},
                new Bson {{"$unwind", "$Posts"}},
                new Bson {{"$sort", new Bson {{"Posts.Meta.Updated", -1}}}},
                new Bson {{"$skip", skip}},
                new Bson {{"$limit", limit}},
                new Bson {{"$group", new Bson {{"_id", BsonNull.Value}, {"Posts", new Bson("$push", "$Posts")}}}},
                new Bson {{"$project", new Bson {{"_id", 0}, {"Posts", 1}}}}
            };

            return (await collection.AggregateAsync(pipeline)).FirstOrDefault().Posts;
        }

        public async Task AppendFollowingPostAsync(string userId, string destId)
        {
            var curror = Context.Posts.Find(x => x.By.Id == destId && x.Meta.Deleted == null)
                .Sort(Builders<Post>.Sort.Descending(x => x.Meta.Created))
                .ToCursor();

            await curror.ForEachAsync(async post =>
            {
                var filter = Builders<Feed>.Filter;
                var exits = await Context.News.CountDocumentsAsync(filter.And(filter.Eq(nameof(Feed.UserId), userId), filter.Eq($"{nameof(Feed.Posts)}._id", post.Id)));
                if (exits == 0)
                    await AppendPostAsync(Context.News, new List<string> { userId }, post);
            });
        }

        public async Task AppendPostAsync(Owner owner, Post post)
        {
            var followers = await Context.Users.Find(x => x.Id == owner.Id).Project(x => x.Followers).ToListAsync();
            var wallDest = new List<string> { owner.Id };
            var newsDest = new List<string> { owner.Id };

            if (followers.All(x => x.Any()))
                newsDest.AddRange(followers.SelectMany(x => x.Keys));

            await Task.WhenAll(AppendPostAsync(Context.Wall, wallDest, post),
                AppendPostAsync(Context.News, newsDest, post));
        }

        public async Task AppendCommentAsync(string postId, Comment comment)
        {
            await Task.WhenAll(
                Context.Wall.UpdateOneAsync(
                    Builders<Feed>.Filter.Eq($"{nameof(Feed.Posts)}._id", ObjectId.Parse(postId)),
                    Builders<Feed>.Update.Push($"{nameof(Feed.Posts)}.$.{nameof(Post.Comments)}", comment)),
                Context.News.UpdateOneAsync(
                    Builders<Feed>.Filter.Eq($"{nameof(Feed.Posts)}._id", ObjectId.Parse(postId)),
                    Builders<Feed>.Update.Push($"{nameof(Feed.Posts)}.$.{nameof(Post.Comments)}", comment))
            );
        }

        async Task<bool> AppendPostAsync(IMongoCollection<Feed> collection, List<string> destUsers, Post post)
        {
            var filter = Builders<Feed>.Filter.In(nameof(Feed.UserId), destUsers);
            var update = Builders<Feed>.Update.Push(nameof(Feed.Posts), post);
            var result = await collection.UpdateManyAsync(filter, update, new UpdateOptions { IsUpsert = true });
            return result.ModifiedCount > 0;
        }

        ResourceDbContext Context { get; }
    }
}