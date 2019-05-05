using MiniBook.Data.Context;
using MiniBook.Data.Entities;
using MiniBook.Data.Repositories;
using Xunit;

namespace MiniBook.Server.Test
{
    public class PostRepositoryTest
    {
        [Fact]
        public async void GetNewsFeed()
        {
            var context = GetContext();

            var results = await new PostRepository(context)
                .GetNewsFeed("65dfafa2-8000-4638-9595-751a55b58e56", 0);
        }
        [Fact]
        public async void PostStatus()
        {
            var context = GetContext();

            await new PostRepository(context)
                 .CreateAsync("74bcd1f9-cf7f-48fe-bef7-be864f5d447e", "hello world");
        }
        [Fact]
        public async void Comment()
        {
            var context = GetContext();

            await new PostRepository(context)
                 .CommentAsync("7e8ae9d5-ce6e-4aaa-be58-50f3c9f01760", "5cce9cde05db474150257fd1", "hello world");
        }

        static ResourceDbContext GetContext()
        {
            return new ResourceDbContext("mongodb://localhost:27017", "MiniBook");
        }
    }
}