using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manga.Application.Boundaries.Login;
using Manga.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manga.WebApi.UseCases.Login
{
    [Route("api/login/[controller]")]
    public class CustomersController : Controller
    {
        private readonly IUseCase loginUseCase;
        private readonly Presenter presenter;

        public CustomersController(
            IUseCase loginUseCase,
            Presenter presenter)
        {
            this.loginUseCase = loginUseCase;
            this.presenter = presenter;
        }

        /// <summary>
        /// Register a new Customer
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginRequest request)
        {
            await loginUseCase.Execute(new Input(
                new Name(request.Name),
                new Password(request.Password)));

            return presenter.ViewModel;
        }
    }
}