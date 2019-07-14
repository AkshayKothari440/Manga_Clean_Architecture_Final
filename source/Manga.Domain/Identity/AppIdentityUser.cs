using Manga.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Manga.Domain.Identity
{
    public class AppIdentityUser : IdentityUser
    {
        [Column]
        [Required]
        public string SSN { get;  set; }
    }
}
