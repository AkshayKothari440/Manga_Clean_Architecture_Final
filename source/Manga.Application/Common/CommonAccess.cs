using Manga.Application.Repositories;
using Manga.Application.Service;
using Manga.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Manga.Application.Common
{
    public static class CommonAccess
    {

        public static async Task<object> GetAccessAccount(string accountId,IAccountRepository accountRepository, ILoginUserService loginUserService)
        {
            Guid AccountId;
            Guid guid;
            bool IsEmail = Regex.IsMatch(accountId, @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.IgnoreCase);
            bool IsPhone = Regex.IsMatch(accountId, @"^[0-9]{10}$", RegexOptions.IgnoreCase);
            bool IsGuid = Guid.TryParse(accountId, out guid);

            if (IsEmail)
            {
                var isEmailExist = await loginUserService.GetEmailUser(accountId);

                if (isEmailExist != null)
                {
                    AccountId = await accountRepository.GetAccountId(Guid.Parse(isEmailExist.Id));
                    return AccountId;
                }
                else
                {
                    return "Email Not Found";
                }

            }

            else if (IsPhone)
            {
                var isPhoneExist = await loginUserService.GetMobileUser(accountId);

                if (isPhoneExist != null)
                {
                    AccountId = await accountRepository.GetAccountId(Guid.Parse(isPhoneExist.Id));
                    return AccountId;
                }
                else
                {
                    return "Mobile Number Is Not Found";
                }

            }
            else if(IsGuid)
            {
                AccountId = Guid.Parse(accountId);
                return AccountId;
            }

            else if (!IsPhone && !IsEmail)
            {
                var isUserNameExist = await loginUserService.GetNameUser(accountId);

                if (isUserNameExist != null)
                {
                    AccountId = await accountRepository.GetAccountId(Guid.Parse(isUserNameExist.Id));
                    return AccountId;
                }
                else
                {
                    return "UserName Is Not Found";
                }

            }
            else
            {
                return "UnExpected Error";
            }
        }
        public static async Task<object> GetAccessCustomer(string customerId, IAccountRepository accountRepository, ILoginUserService loginUserService)
        {
            Guid CustomerId;
            Guid guid;
            bool IsEmail = Regex.IsMatch(customerId, @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.IgnoreCase);
            bool IsPhone = Regex.IsMatch(customerId, @"^[0-9]{10}$", RegexOptions.IgnoreCase);
            bool IsGuid = Guid.TryParse(customerId, out guid);

            if (IsEmail)
            {
                var isEmailExist = await loginUserService.GetEmailUser(customerId);

                if (isEmailExist != null)
                {
                    CustomerId = Guid.Parse(isEmailExist.Id);
                    return CustomerId;
                }
                else
                {
                    return "Email Not Found";
                }

            }

            else if (IsPhone)
            {
                var isPhoneExist = await loginUserService.GetMobileUser(customerId);

                if (isPhoneExist != null)
                {
                    CustomerId = Guid.Parse(isPhoneExist.Id);
                    return CustomerId;
                }
                else
                {
                    return "Mobile Number Is Not Found";
                }

            }
            else if (IsGuid)
            {
                CustomerId = Guid.Parse(customerId);
                return CustomerId;
            }

            else if (!IsPhone && !IsEmail && !IsGuid)
            {
                var isUserNameExist = await loginUserService.GetNameUser(customerId);

                if (isUserNameExist != null)
                {
                    CustomerId = Guid.Parse(isUserNameExist.Id);
                    return CustomerId;
                }
                else
                {
                    return "UserName Is Not Found";
                }

            }
            else
            {
                return "UnExpected Error";
            }
        }
    }
}
