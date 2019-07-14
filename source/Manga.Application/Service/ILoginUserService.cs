using Manga.Application.Boundaries.Login;
using Manga.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manga.Application.Services
{
    public interface ILoginUserService
    {
        LoginOutput Execute(AppIdentityUser user, string password);
        Task<AppIdentityUser> GetEmailUser(string email);
        Task<AppIdentityUser> GetNameUser(string name);
        Task<AppIdentityUser> GetMobileUser(string mobile);
    }
}