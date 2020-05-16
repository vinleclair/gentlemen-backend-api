using System.Linq;
using System.Threading.Tasks;
using Gentlemen.Features.Users;
using Gentlemen.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Gentlemen.Tests.Features.Users
{
    public class CreateTests : TestFixture
    {
        [Fact]
        public async Task Expect_Create_User()
        {
            var command = new Create.Command()
            {
                User = new Create.UserData()
                {
                    Email = "john_doe@example.com",
                    Password = "hunter2",
                }
            };

            await SendAsync(command);

            var created = await ExecuteDbContextAsync(db => db.Customers.Where(d => d.Email == command.User.Email).SingleOrDefaultAsync());

            Assert.NotNull(created);
            Assert.Equal(created.Hash, new PasswordHasher().Hash(command.User.Password, created.Salt));
        }
    }
}