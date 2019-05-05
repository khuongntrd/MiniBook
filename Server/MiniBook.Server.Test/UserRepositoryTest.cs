
using System;
using MiniBook.Data.Context;
using MiniBook.Data.Repositories;
using Xunit;

namespace MiniBook.Server.Test
{
    public class UserRepositoryTest
    {
        [Fact]
        public async void Follow()
        {
            var context = GetContext();

            var currentUser = "22f87da1-b460-40dd-b028-3e370e02ec6d";
            var destUser = "7e8ae9d5-ce6e-4aaa-be58-50f3c9f01760";

            if (await new UserRepository(context).FollowAsync(currentUser, destUser))
                await new FeedRepository(context).AppendFollowingPostAsync(currentUser, destUser);
        }

        [Fact]
        public async void UpdateProfile()
        {
            var repo = GetRepo();

            Assert.True(await repo.UpdateAsync("65dfafa2-8000-4638-9595-751a55b58e56", "Kirk Davis", "Male",
                "https://randomuser.me/api/portraits/men/78.jpg"));


        }
        static ResourceDbContext GetContext()
        {
            return new ResourceDbContext("mongodb://localhost:27017", "MiniBook");
        }

        static UserRepository GetRepo()
        {
            var repo = new UserRepository(GetContext());
            return repo;
        }
    }
}
