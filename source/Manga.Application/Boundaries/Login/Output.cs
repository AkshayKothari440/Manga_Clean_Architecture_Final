using System;
using System.Collections.Generic;
using System.Text;

namespace Manga.Application.Boundaries.Login
{
    public class Output
    {
        public Guid CustomerId { get; set; }

        public string Name { get; set; }
        public string Token { get; set; }

        public Output(Guid customerId, string name, string token)
        {
            CustomerId = customerId;
            Name = name;
            Token = token;
        }
    }
}