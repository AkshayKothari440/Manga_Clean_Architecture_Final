using Manga.Application.Boundaries.Login;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manga.WebApi.UseCases.Login
{
    public class Presenter : IOutputHandler
    {
        public IActionResult ViewModel { get; private set; }

        public void Error(string message)
        {
            ViewModel = new ObjectResult(message);
        }

        public void Handle(Output output)
        {
            CustomerModel model = new CustomerModel(
                output.CustomerId,
                output.Name,
                output.Token
            );

            ViewModel = new CreatedAtRouteResult("GetCustomer",
                new
                {
                    customerId = model.CustomerId
                },
                model);
        }
    }
}