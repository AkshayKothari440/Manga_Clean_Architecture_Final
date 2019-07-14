namespace Manga.WebApi.UseCases.Deposit
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Manga.Application.Boundaries.Deposit;
    using Manga.Domain.ValueObjects;
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    [Route("api/[controller]")]
        
    public class AccountsController : Controller
    {
        private readonly IUseCase _depositUseCase;
        private readonly Presenter _presenter;

        //===========================================================//
        //private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly IConfiguration _configuration;
        //===========================================================//
        public AccountsController(
            IUseCase depositUseCase,
            Presenter presenter)
        {
            _depositUseCase = depositUseCase;
            _presenter = presenter;
        }
    

    //=========================================================================//
    //[HttpPost]
    //public async Task<object> Login([FromBody] LoginDto model)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest(ModelState);
    //    }

    //    bool IsEmail = Regex.IsMatch(model.EmailOrUserNameOrPhone, @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.IgnoreCase);
    //    bool IsPhone = Regex.IsMatch(model.EmailOrUserNameOrPhone, @"^[0-9]{10}$", RegexOptions.IgnoreCase);

    //    if (IsEmail)
    //    {
    //        var isEmailExist = await _userManager.FindByEmailAsync(model.EmailOrUserNameOrPhone);

    //        if (isEmailExist != null)
    //        {
    //            return await AttemptLogin(isEmailExist, model.Password);
    //        }
    //        else
    //        {
    //            return "INVALID_EMAIL_ADDRESS";
    //        }

    //    }

    //    if (IsPhone)
    //    {
    //        var isPhoneExist = _userManager.Users.SingleOrDefault(p => p.PhoneNumber == model.EmailOrUserNameOrPhone);

    //        if (isPhoneExist != null)
    //        {
    //            return await AttemptLogin(isPhoneExist, model.Password);
    //        }
    //        else
    //        {
    //            return "INVALID_PHONE_NUMBER";
    //        }

    //    }

    //    if (!IsPhone && !IsEmail)
    //    {
    //        var isUserNameExist = await _userManager.FindByNameAsync(model.EmailOrUserNameOrPhone);

    //        if (isUserNameExist != null)
    //        {
    //            return await AttemptLogin(isUserNameExist, model.Password);
    //        }
    //        else
    //        {
    //            return "INVALID_USER_NAME";
    //        }

    //    }

    //    return "UNEXPEXTED_ERROR";
    //    //throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
    //}

    //private async Task<Object> AttemptLogin(IdentityUser appUser, string Password)
    //{
    //    var result = await _signInManager.PasswordSignInAsync(appUser, Password, false, false);
    //    if (result.Succeeded)
    //    {
    //        //var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.EmailOrUserNameOrPhone);
    //        return await GenerateJwtToken(appUser.Email, appUser);
    //    }
    //    else if (result.IsLockedOut)
    //    {
    //        return "YOUR_ACCOUNT_IS_LOCKED";
    //    }
    //    else if (result.IsNotAllowed)
    //    {
    //        return "YOU_ARE_NOT_ALLOWED_TO_ATTEMPT_LOGIN";
    //    }
    //    else if (result.RequiresTwoFactor)
    //    {
    //        return "YOUR_ACCOUNT_IS_REQUIRED_TWO_FACTOR_AUTHENTICATION";
    //    }
    //    else
    //    {
    //        return "INVALID_PASSWORD";
    //    }
    //}
    // private async Task<object> GenerateJwtToken(string email, IdentityUser user)
    //{
    //    var claims = new List<Claim>
    //    {
    //        new Claim(JwtRegisteredClaimNames.Sub, email),
    //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //        new Claim(ClaimTypes.NameIdentifier, user.Id)
    //    };

    //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
    //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    //    var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

    //    var token = new JwtSecurityToken(
    //        _configuration["JwtIssuer"],
    //        _configuration["JwtIssuer"],
    //        claims,
    //        expires: expires,
    //        signingCredentials: creds
    //    );

    //    return new JwtSecurityTokenHandler().WriteToken(token);
    //}
    //=========================================================================//
    /// <summary>
    /// Deposit to an account
    /// </summary>
    [HttpPatch("Deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositRequest message)
        {
            await _depositUseCase.Execute(new Input(message.AccountId, new PositiveAmount(message.Amount)));
            return _presenter.ViewModel;
        }
    }
}