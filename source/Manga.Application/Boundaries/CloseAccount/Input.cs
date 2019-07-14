namespace Manga.Application.Boundaries.CloseAccount
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