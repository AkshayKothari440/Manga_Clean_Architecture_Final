using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manga.WebApi.UseCases.Login
{
    public class CustomerModel
    {
        public Guid CustomerId { get; }
        public string Name { get; }
        public string Token { get; set; }

        public CustomerModel(
            Guid customerId,
            string name,
            string token)
        {
            CustomerId = customerId;
            Name = name;
            Token = token;
        }
    }
}