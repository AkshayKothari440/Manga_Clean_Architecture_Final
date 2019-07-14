namespace Manga.WebApi.UseCases.Withdraw
{
    using System;
    public class WithdrawRequest
    {
        public string AccountId { get; set; }
        public Double Amount { get; set; }
    }
}