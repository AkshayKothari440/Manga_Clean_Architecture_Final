namespace Manga.Application.UseCases
{
    using System;
    using System.Threading.Tasks;
    using Manga.Application.Boundaries.GetAccountDetails;
    using Manga.Application.Common;
    using Manga.Application.Repositories;
    using Manga.Application.Services;
    using Manga.Domain.Accounts;

    public sealed class GetAccountDetails : IUseCase
    {
        private readonly IOutputHandler _outputHandler;
        private readonly IAccountRepository _accountRepository;
        private readonly ILoginUserService _loginUserService;

        public GetAccountDetails(
            IOutputHandler outputHandler,
            IAccountRepository accountRepository,
            ILoginUserService loginUserService
            )
        {
            _outputHandler = outputHandler;
            _accountRepository = accountRepository;
            _loginUserService = loginUserService;

        }

    public async Task Execute(Input input)
        {
            Guid AccountId;
            var result = CommonAccess.GetAccessAccount(input.AccountId, _accountRepository, _loginUserService).Result.ToString();
            if(!Guid.TryParse(result,out AccountId))
            {
                _outputHandler.Error(result);
                return;
            }
            IAccount account = await _accountRepository.Get(AccountId);

            if (account == null)
            {
                _outputHandler.Error($"The account {input.AccountId} does not exists or is not processed yet.");
                return;
            }

            Output output = new Output(account);
            _outputHandler.Handle(output);
        }
    }
}