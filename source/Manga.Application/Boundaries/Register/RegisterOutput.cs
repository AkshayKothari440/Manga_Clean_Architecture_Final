using Manga.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manga.Application.Boundaries.Register
{
   public class RegisterOutput
    {
        public AppIdentityUser user { get; set; }
        public string Token { get; set; }
    }
}
