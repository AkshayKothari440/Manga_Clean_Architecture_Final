using System;
using System.Collections.Generic;
using System.Text;

namespace Manga.Application.Boundaries.Login
{
    public class LoginOutput
    {
        public Guid CustomerId { get; set; }

        public string Name { get; set; }
        public string Token { get; set; }
    }
}