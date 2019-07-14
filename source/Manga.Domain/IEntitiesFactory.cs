namespace Manga.Domain
{
    using System;
    using Manga.Domain.Accounts;
    using Manga.Domain.Customers;
    using Manga.Domain.ValueObjects;

    public interface IEntitiesFactory
    {
        ICustomer NewCustomer(Guid customerId,SSN ssn, Name name);
        IAccount NewAccount(Guid customerId);
    }
}