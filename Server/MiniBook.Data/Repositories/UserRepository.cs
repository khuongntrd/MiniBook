using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniBook.Data.Context;
using MiniBook.Data.Entities;
using MongoDB.Driver;

namespace MiniBook.Data.Repositories
{
    public class UserRepository
    {
        public UserRepository(ResourceDbContext context)
        {
            Context = context;
        }

        public async Task<bool> UpdateAsync(string userId, string name, string gender, string image)
        {
            var profile = new Profile(name, image, gender);

            if (!await UpdateAsync(userId, Builders<User>.Update.Set(nameof(Profile), profile), false))
                return false;

            await Task.WhenAll(
                UpdateFollowerProfile(userId, profile),
                UpdatePostOwner(userId, profile),
                UpdateCommentOwner(userId, profile));

            return true;

        }

        public Task CreateAsync(string userId, string name, string gender, string image)
        {
            var user = new User
            {
                Id = userId,
                Profile = new Profile(name, image, gender)
            };

            return Context.Users.InsertOneAsync(user);
        }

        public Task<List<User>> SearchAsync(string name)
        {
            return Context.Users.Find(Builders<User>.Filter.Text(name)).ToListAsync();
        }

        public async Task<bool> FollowAsync(string userId, string destId)
        {
            var profile = await Context.Users.Find(x => x.Id == userId)
                .Project(x => x.Profile)
                .SingleOrDefaultAsync();

            if (profile != null)
                return await UpdateAsync(destId, Builders<User>.Update
                    .Set($"{nameof(User.Followers)}.{userId}", profile));

            return false;
        }

        Task UpdateFollowerProfile(string userId, Profile profile)
        {
            var followerPath = $"{nameof(User.Followers)}.{userId}";

            return Context.Users.UpdateManyAsync(Builders<User>.Filter.Exists(followerPath),
                Builders<User>.Update.Set(followerPath, profile));
        }

        Task UpdatePostOwner(string userId, Profile profile)
        {
            return Context.Posts.UpdateManyAsync(Builders<Post>.Filter.Eq($"{nameof(Post.By)}._id", userId),
                Builders<Post>.Update.Set($"{nameof(Post.By)}", new Owner(userId, profile)));
        }

        Task UpdateCommentOwner(string userId, Profile profile)
        {
            var commentPath = $"{nameof(Post.Comments)}.{nameof(Post.By)}";

            return Context.Posts.UpdateManyAsync(Builders<Post>.Filter.Eq(commentPath + "._id", userId),
                Builders<Post>.Update.Set(commentPath, new Owner(userId, profile)));
        }

        async Task<bool> UpdateAsync(string id, UpdateDefinition<User> update, bool isUpsert = true)
        {
            update = update.Set("Meta.Updated", DateTime.UtcNow);
            var filter = Builders<User>.Filter.Eq("_id", id);
            var result = await Context.Users.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = isUpsert });
            return result.ModifiedCount > 0;
        }

        ResourceDbContext Context { get; }
    }
}