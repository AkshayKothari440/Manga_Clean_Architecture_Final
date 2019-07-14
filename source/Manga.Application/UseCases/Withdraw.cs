namespace Manga.Application.UseCases
{
    using System;
    using System.Threading.Tasks;
    using Manga.Application.Boundaries.Withdraw;
    using Manga.Application.Common;
    using Manga.Application.Repositories;
    using Manga.Application.Services;
    using Manga.Domain.Accounts;

    public sealed class Withdraw : IUseCase
    {
        private readonly IOutputHandler _outputHandler;
        private readonly IAccountRepository _accountRepository;
        private readonly ILoginUserService _loginUserService;

        public Withdraw(
            IOutputHandler outputHandler,
            IAccountRepository accountRepository,
            ILoginUserService loginUserService)
        {
            _outputHandler = outputHandler;
            _accountRepository = accountRepository;
            _loginUserService = loginUserService;
        }

        public async Task Execute(Input input)
        {
            Guid AccountId;
            var result = CommonAccess.GetAccessAccount(input.AccountId, _accountRepository, _loginUserService).Result.ToString();
            if (!Guid.TryParse(result, out AccountId))
            {
                _outputHandler.Error(result);
                return;
            }
            IAccount account = await _accountRepository.Get(AccountId);
            if (account == null)
            {
                _outputHandler.Error($"The account {input.AccountId} does not exists or is already closed.");
                return;
            }

            IDebit debit = account.Withdraw(input.Amount);

            if (debit == null)
            {
                _outputHandler.Error($"The account {input.AccountId} does not have enough funds to withdraw {input.Amount}.");
                return;
            }

            await _accountRepository.Update(account, debit);

            Output output = new Output(
                debit,
                account.GetCurrentBalance()
            );

            _outputHandler.Handle(output);
        }
    }
}