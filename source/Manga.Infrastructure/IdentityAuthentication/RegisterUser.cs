using Manga.Application.Authentication;
using Manga.Application.Boundaries.Register;
using Manga.Application.Service;
using Manga.Domain.Customers;
using Manga.Domain.Identity;
using Manga.Domain.ValueObjects;
using Manga.Infrastructure.EntityFrameworkDataAccess;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Manga.Infrastructure.IdentityAuthentication
{
    public sealed class RegisterUser: IRegisterUserService
    {
        private readonly UserManager<AppIdentityUser> userManager;
        private readonly SignInManager<AppIdentityUser> signInManager;
        private readonly IGenerateToken generateToken;
        private readonly MangaContext mangaContext;

        private RegisterOutput Output { get; set; }
        public RegisterUser(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager, IGenerateToken generateToken, MangaContext mangaContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.generateToken = generateToken;
            this.mangaContext = mangaContext;
        }
        public RegisterOutput Execute(Input input)
        {
            return RegistrationAsync(input).Result;
        }
        public async Task<AppIdentityUser> GetEmailUser(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }
        public async Task<AppIdentityUser> GetNameUser(string name)
        {
            return await userManager.FindByNameAsync(name);
        }
        public async Task<AppIdentityUser> GetMobileUser(string mobile)
        {
            return userManager.Users.SingleOrDefault(m => m.PhoneNumber == mobile);
        }
        public async Task<ICustomer> GetCustomer(Guid id)
        {
            var user = userManager.FindByIdAsync(id.ToString());

            Domain.Customers.Customer customer = new Domain.Customers.Customer(Guid.Parse(user.Result.Id),new SSN(user.Result.SSN),new Name(user.Result.UserName));

            var accounts = mangaContext.Accounts
                .Where(e => e.CustomerId == id)
                .Select(e => e.Id)
                .ToList();

            customer.LoadAccounts(accounts);

            return customer;
        }
        private async Task<RegisterOutput> RegistrationAsync(Input input)
        {
            var user = new AppIdentityUser { Id = Guid.NewGuid().ToString() ,UserName = input.Name.ToString(),Email=input.Email.ToString(),PhoneNumber=input.Mobile.ToString(),SSN = input.SSN.ToString()};
            var Result = await userManager.CreateAsync(user, input.Password.ToString());
            if(Result.Succeeded)
            {
                //var debug = await generateToken.GetToken(username, user);
                //return new Guid(user.Id);
                await signInManager.SignInAsync(user, isPersistent: false);

                var token = await generateToken.GetToken(input.Name.ToString(), user);


                return new RegisterOutput { user = user, Token = token };
            }
            return null;
        }
    }
}
