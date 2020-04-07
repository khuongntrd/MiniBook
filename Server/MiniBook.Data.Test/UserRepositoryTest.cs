using MiniBook.Data.Context;
using MiniBook.Data.Repositories;
using Xunit;

namespace MiniBook.Data.Test
{
    public class PostRepositoryTest
    {
        [Fact]
        public async void GetFeed()
        {
            var repo = GetRespository();

            var results = await repo.GetWallFeed("de430bd6-aaa1-4d89-a28c-e4e83d17af5f", 1);
        }
        [Fact]
        public async void CreatePost()
        {
            var repo = GetRespository();

            await repo.CreateAsync("de430bd6-aaa1-4d89-a28c-e4e83d17af5f", "Welcome to Xamarin");
        }
        [Fact]
        public async void CommentPost()
        {
            var repo = GetRespository();

            await repo.CommentAsync("74bcd1f9-cf7f-48fe-bef7-be864f5d447e", "5cd6ec32c279004f70ff7804", "like");
        }

        public static ResourceDbContext GetDbContext()
        {
            return new ResourceDbContext("mongodb://localhost:27017", "MiniBook");
        }

        public static PostRepository GetRespository()
        {
            return new PostRepository(GetDbContext());
        }

    }
    public class UserRepositoryTest
    {
        [Fact]
        public async void Follow()
        {
            var repo = GetRespository();
            await repo.FollowAsync("74bcd1f9-cf7f-48fe-bef7-be864f5d447e", "de430bd6-aaa1-4d89-a28c-e4e83d17af5f");
        }

        public static ResourceDbContext GetDbContext()
        {
            return new ResourceDbContext("mongodb://localhost:27017", "MiniBook");
        }
        public static UserRepository GetRespository()
        {
            return new UserRepository(GetDbContext());
        }
    }
}
