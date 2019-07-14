using Manga.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Manga.Application.Authentication
{
    public interface IGenerateToken
    {
        Task<string> GetToken(string username, AppIdentityUser user);
    }
}
