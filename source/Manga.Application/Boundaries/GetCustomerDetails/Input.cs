namespace Manga.Application.Boundaries.GetCustomerDetails
{
    using System;

    public sealed class Input
    {
        public string CustomerId { get; }

        public Input(string customerId)
        {
            CustomerId = customerId;
        }
    }
}