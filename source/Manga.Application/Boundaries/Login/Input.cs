using Manga.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manga.Application.Boundaries.Login
{
    public class Input
    {
        public object Name { get; }

        public Password Password { get; }

        public Input(Name name, Password password)
        {
            Name = name;
            Password = password;
        }
    }
}