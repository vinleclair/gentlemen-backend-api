using System;
using System.Threading.Tasks;
using Gentlemen.Domain;
using Gentlemen.Features.Users;
using Gentlemen.Infrastructure.Security;
using Xunit;

namespace Gentlemen.Tests.Features.Users
{
    public class LoginTests : TestFixture
    {
        [Fact]
        public async Task Expect_Login()
        {
            var salt = Guid.NewGuid().ToByteArray();
            var customer = new Customer
            {
                Email = "john_doe@example.com",
                Hash = new PasswordHasher().Hash("hunter2", salt),
                Salt = salt
            };
            await InsertAsync(customer);

            var command = new Login.Command()
            {
                User = new Login.UserData()
                {
                    Email = "john_doe@example.com",
                    Password = "hunter2"
                }
            };

            var user = await SendAsync(command);

            Assert.NotNull(user?.User);
            Assert.Equal(user.User.Email, command.User.Email);
            Assert.NotNull(user.User.Token);
        }
    }
}