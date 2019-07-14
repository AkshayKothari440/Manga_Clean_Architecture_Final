namespace Manga.Application.Boundaries.GetAccountDetails
{
    using System;

    public sealed class Input
    {
        public string AccountId { get; }

        public Input(string accountId)
        {
            AccountId = accountId;
        }
    }
}