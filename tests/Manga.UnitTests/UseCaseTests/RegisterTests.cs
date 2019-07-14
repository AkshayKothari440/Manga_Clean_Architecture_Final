namespace Manga.UnitTests.UseCasesTests
{
    using Xunit;
    using Manga.Application.UseCases;
    using Manga.Domain;
    using Manga.Infrastructure.InMemoryGateway;
    using Manga.Infrastructure.InMemoryDataAccess.Repositories;
    using Manga.Infrastructure.InMemoryDataAccess;
    using System.Linq;
    using Manga.Domain.ValueObjects;
    using System.Threading.Tasks;
    using Application.Boundaries.Register;
    using System;
    using Manga.Infrastructure.IdentityAuthentication;
    using Microsoft.AspNetCore.Identity;
    using Manga.Domain.Identity;

    public sealed class RegisterTests
    {
        [Fact]
        public void GivenNullInput_ThrowsException()
        {
            var register = new Register(null, null, null, null,null);
            Assert.ThrowsAsync<Exception>(async() => await register.Execute(null));
        }

        [Theory]
        [InlineData(300)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(3300)]
        public async Task Register_WritesOutput_InputIsValid(double amount)
        {
            var ssn = new SSN("9909999099");
            var name = new Name("hello");
            var password = new Password("hello@123");

            var entityFactory = new DefaultEntitiesFactory();
            var presenter = new Presenter();
            var context = new MangaContext();
            var customerRepository = new CustomerRepository(context);
            var accountRepository = new AccountRepository(context);
            var registerUser = new RegisterUser(new UserManager<AppIdentityUser>());

            var sut = new Register(
                entityFactory,
                presenter,
                customerRepository,
                accountRepository,
                registerUser
            );

            await sut.Execute(new Input(
                ssn,
                name,
                password,
                new PositiveAmount(amount)));
            
            var actual = presenter.Registers.First();
            Assert.NotNull(actual);
            Assert.Equal(ssn.ToString(), actual.Customer.SSN);
            Assert.Equal(name.ToString(), actual.Customer.Name);
            Assert.Equal(amount, actual.Account.CurrentBalance);
        }
    }
}
