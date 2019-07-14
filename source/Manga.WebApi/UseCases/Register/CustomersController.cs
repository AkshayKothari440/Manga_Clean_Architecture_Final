namespace Manga.WebApi.UseCases.Register
{
    using System.Threading.Tasks;
    using Manga.Application.Boundaries.Register;
    using Manga.Domain.ValueObjects;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    // [Authorize]
    //[Route("api/[controller]")]
    [Route("api/register/[controller]")]
    public class CustomersController : Controller
    {
        private readonly IUseCase _registerUseCase;
        private readonly Presenter _presenter;

        public CustomersController(
            IUseCase registerUseCase,
            Presenter presenter)
        {
            _registerUseCase = registerUseCase;
            _presenter = presenter;
        }

        /// <summary>
        /// Register a new Customer
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterRequest request)
        {
            await _registerUseCase.Execute(new Input(
                new SSN(request.SSN),
                new Name(request.Name),
                new Email(request.Email),
                new Mobile(request.Mobile),
                new Password(request.Password),
                new PositiveAmount(request.InitialAmount)));

            return _presenter.ViewModel;
        }
    }
}