using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manga.WebApi.UseCases.Login
{
    public class LoginRequest
    {
        public string Name { get; set; }

        public string Password { get; set; }
    }
}