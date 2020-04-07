using MiniBook.Data.Context;
using MiniBook.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBook.Data.Repositories
{
    public class PostRepository : FeedRepository
    {
        public PostRepository(ResourceDbContext context) : base(context)
        {

        }

        public Task CreateAsync(string userId, string text, params Photo[] photos)
        {
            var profile = Context.Users.Find(x => x.Id == userId)
                .Project(x => x.Profile)
                .SingleOrDefault();

            if (profile != null)
            {
                if (photos?.Any(x => x.Url != null) == true)
                    return CreateAsync(new Owner(userId, profile), text, photos.ToList());

                return CreateAsync(new Owner(userId, profile), text);
            }

            return Task.CompletedTask;
        }

        public Task CreateAsync(Owner owner, string text, List<Photo> photos)
        {
            if (photos == null)
                throw new ArgumentNullException(nameof(photos));

            var post = new Post
            {
                By = owner,
                Detail = new Detail { Text = text, Photos = photos },
                Type = PostType.Photo,
            };

            return CreateAsync(post);
        }

        public async Task CreateAsync(Owner owner, string text)
        {
            var post = new Post
            {
                By = owner,
                Detail = new Detail { Text = text },
                Type = PostType.Status
            };

            await CreateAsync(post);
        }

        async Task CreateAsync(Post post)
        {
            await Context.Posts.InsertOneAsync(post);

            await AppendPostAsync(post);
        }

        public Task CommentAsync(string userId, string postId, string text)
        {
            var profile = Context.Users.Find(x => x.Id == userId)
                .Project(x => x.Profile).SingleOrDefault();

            if (profile != null)
                return CommentAsync(new Owner(userId, profile), postId, text);

            return Task.CompletedTask;
        }

        public async Task CommentAsync(Owner user, string postId, string text)
        {
            var comment = new Comment { By = user, Text = text, Ts = DateTime.UtcNow };

            await Context.Posts.UpdateOneAsync(
                Builders<Post>.Filter.Eq("_id", ObjectId.Parse(postId)),
                Builders<Post>.Update.Push(nameof(Post.Comments), comment));

            await AppendCommentAsync(postId, comment);
        }
    }
}
