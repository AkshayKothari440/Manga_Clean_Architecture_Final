namespace Manga.Domain
{
    using System;
    using Manga.Domain.Accounts;
    using Manga.Domain.Customers;
    using Manga.Domain.ValueObjects;

    public sealed class DefaultEntitiesFactory : IEntitiesFactory
    {
        public IAccount NewAccount(Guid customerId)
        {
            var account = new Account(customerId);
            return account;
        }

        public ICustomer NewCustomer(Guid customerId,SSN ssn, Name name)
        {
            var customer = new Customer(customerId,ssn, name);
            return customer;
        }
    }
}