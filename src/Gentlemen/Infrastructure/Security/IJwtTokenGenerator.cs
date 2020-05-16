using System.Threading.Tasks;

namespace Gentlemen.Infrastructure.Security
{
    public interface IJwtTokenGenerator
    {
        Task<string> CreateToken();
    }
}