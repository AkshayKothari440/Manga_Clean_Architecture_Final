namespace Manga.Application.UseCases
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using Manga.Application.Boundaries.GetCustomerDetails;
    using Manga.Application.Repositories;
    using Manga.Domain.Accounts;
    using Manga.Domain.Customers;
    using Manga.Application.Service;
    using Manga.Application.Services;
    using Manga.Application.Common;

    public sealed class GetCustomerDetails : IUseCase
    {
        private readonly IOutputHandler _outputHandler;
        private readonly IRegisterUserService registerUserService;
        //private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ILoginUserService _loginUserService;
        public GetCustomerDetails(
            IOutputHandler outputHandler,
            IRegisterUserService registerUserService,
            //ICustomerRepository customerRepository,
            IAccountRepository accountRepository,
            ILoginUserService loginUserService)
        {
            _outputHandler = outputHandler;
            this.registerUserService = registerUserService;
            //_customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _loginUserService = loginUserService;
        }

        public async Task Execute(Input input)
        {
            Guid CustomerId;
            var result = CommonAccess.GetAccessCustomer(input.CustomerId, _accountRepository, _loginUserService).Result.ToString();
            if (!Guid.TryParse(result, out CustomerId))
            {
                _outputHandler.Error(result);
                return;
            }
            ICustomer customer = await registerUserService.GetCustomer(CustomerId);

            if (customer == null)
            {
                _outputHandler.Error($"The customer {CustomerId} does not exists or is not processed yet.");
                return;
            }

            List<Boundaries.GetCustomerDetails.Account> accounts = new List<Boundaries.GetCustomerDetails.Account>();

            foreach (Guid accountId in customer.Accounts)
            {
                IAccount account = await _accountRepository.Get(accountId);

                if (account != null)
                {
                    Boundaries.GetCustomerDetails.Account accountOutput = new Boundaries.GetCustomerDetails.Account(account);
                    accounts.Add(accountOutput);
                }
            }

            Output output = new Output(customer, accounts);
            _outputHandler.Handle(output);
        }
    }
}