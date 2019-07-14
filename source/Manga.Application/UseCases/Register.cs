namespace Manga.Application.UseCases
{
    using System.Threading.Tasks;
    using Manga.Application.Boundaries.Register;
    using Manga.Application.Repositories;
    using Manga.Domain.Accounts;
    using Manga.Domain;
    using Manga.Application.Service;
    using System;
    using Microsoft.AspNetCore.Identity;

    public sealed class Register : IUseCase
    {
        private readonly IEntitiesFactory _entityFactory;
        private readonly IOutputHandler _outputHandler;
        //private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRegisterUserService _registerUserService; //Add 

        public Register(
            IEntitiesFactory entityFactory,
            IOutputHandler outputHandler,
            //ICustomerRepository customerRepository,
            IAccountRepository accountRepository, IRegisterUserService registerUserService)
        {
            _entityFactory = entityFactory;
            _outputHandler = outputHandler;
            //_customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _registerUserService = registerUserService; //add

        }

        public async Task Execute(Input input)
        {
            if (input == null)
            {
                _outputHandler.Error("Input is null.");
                return;
            }
            var isEmailExist = await _registerUserService.GetEmailUser(input.Email.ToString());
            var isPhoneExist =  _registerUserService.GetMobileUser(input.Mobile.ToString()).Result;
            var isUserNameExist = await _registerUserService.GetNameUser(input.Name.ToString());
            if (isEmailExist != null)
            {
                _outputHandler.Error("Email Already Exist");
            }
            else if (isPhoneExist != null)
            {
                _outputHandler.Error("MobileNumber Already Exist");
            }
            else if (isUserNameExist != null)
            {
                _outputHandler.Error("UserName Already Exist");
            }
            //var customerId = _registerUserService.Execute(input.Name.ToString(), input.Password.ToString()); //add
            //if (customerId == null || customerId == Guid.Empty) //add
            else if(isEmailExist==null && isPhoneExist==null && isUserNameExist==null)
            {
                var registerOutput = _registerUserService.Execute(input);
                if (registerOutput == null)
                {
                    _outputHandler.Error("An error throw when registering user ID");//add
                    return; //add
                }

                var customer = _entityFactory.NewCustomer(Guid.Parse(registerOutput.user.Id), input.SSN, input.Name);
                var account = _entityFactory.NewAccount(customer.Id);

                ICredit credit = account.Deposit(input.InitialAmount);
                if (credit == null)
                {
                    _outputHandler.Error("An error happened when depositing the amount.");
                    return;
                }

                customer.Register(account.Id);

                //await _customerRepository.Add(customer);
                await _accountRepository.Add(account, credit);

                Output output = new Output(customer, account, registerOutput.Token);
                _outputHandler.Handle(output);
            }
        }
    }
}