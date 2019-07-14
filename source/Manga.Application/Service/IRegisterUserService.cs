using Manga.Application.Boundaries.Register;
using Manga.Domain.Customers;
using Manga.Domain.Identity;
using System;
using System.Threading.Tasks;

namespace Manga.Application.Service
{
    public interface IRegisterUserService
    {
        //Guid Execute(string username, string password);
        RegisterOutput Execute(Input input);
        Task<ICustomer> GetCustomer(Guid id);
        Task<AppIdentityUser> GetEmailUser(string email);
        Task<AppIdentityUser> GetNameUser(string name);
        Task<AppIdentityUser> GetMobileUser(string mobile);
    }
}
