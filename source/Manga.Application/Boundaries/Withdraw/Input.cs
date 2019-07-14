namespace Manga.Application.Boundaries.Withdraw
{
    using System;
    using Manga.Domain.ValueObjects;

    public sealed class Input
    {
        public string AccountId { get; }
        public PositiveAmount Amount { get; }

        public Input(string accountId, PositiveAmount amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}