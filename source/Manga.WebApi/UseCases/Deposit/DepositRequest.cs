namespace Manga.WebApi.UseCases.Deposit
{
    using System;
    public class DepositRequest
    {
        public string AccountId { get; set; }
        public Double Amount { get; set; }
    }
}