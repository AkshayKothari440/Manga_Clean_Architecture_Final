using Manga.Application.Boundaries.Login;
using Manga.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Manga.Application.UseCases
{
    public class Login : IUseCase
    {
        private readonly IOutputHandler outputHandler;
        private readonly ILoginUserService loginUserService;

        public Login(IOutputHandler outputHandler,
            ILoginUserService loginUserService)
        {
            this.outputHandler = outputHandler;
            this.loginUserService = loginUserService;
        }

        public async Task Execute(Input input)
        {
            if (input == null)
            {
                outputHandler.Error("Input is null.");
                return;
            }
            bool IsEmail = Regex.IsMatch(input.Name.ToString(), @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.IgnoreCase);
            bool IsPhone = Regex.IsMatch(input.Name.ToString(), @"^[0-9]{10}$", RegexOptions.IgnoreCase);

            if (IsEmail)
            {
                var isEmailExist = await loginUserService.GetEmailUser(input.Name.ToString());

                if (isEmailExist != null)
                {
                    var loginOutput = loginUserService.Execute(isEmailExist, input.Password.ToString());
                    if (loginOutput == null)
                    {
                        outputHandler.Error("An error throw when Login with user password");
                        return;
                    }

                    Output output = new Output(loginOutput.CustomerId, loginOutput.Name, loginOutput.Token);
                    outputHandler.Handle(output);
                }
                else
                {
                    outputHandler.Error("Email Not Found");
                }

            }

            else if (IsPhone)
            {
                var isPhoneExist = await loginUserService.GetMobileUser(input.Name.ToString());

                if (isPhoneExist != null)
                {
                    var loginOutput = loginUserService.Execute(isPhoneExist, input.Password.ToString());
                    if (loginOutput == null)
                    {
                        outputHandler.Error("An error throw when Login with user password");
                        return;
                    }

                    Output output = new Output(loginOutput.CustomerId, loginOutput.Name, loginOutput.Token);
                    outputHandler.Handle(output);
                }
                else
                {
                    outputHandler.Error("Mobile Number Is Not Found");
                }

            }

            else if (!IsPhone && !IsEmail)
            {
                var isUserNameExist = await loginUserService.GetNameUser(input.Name.ToString());

                if (isUserNameExist != null)
                {
                    var loginOutput = loginUserService.Execute(isUserNameExist, input.Password.ToString());
                    if (loginOutput == null)
                    {
                        outputHandler.Error("An error throw when Login with user password");
                        return;
                    }

                    Output output = new Output(loginOutput.CustomerId, loginOutput.Name, loginOutput.Token);
                    outputHandler.Handle(output);
                }
                else
                {
                    outputHandler.Error("UserName Is Not Found");
                }

            }
            else
            {
                outputHandler.Error("UnExpected Error");
            }
        }
    }
}