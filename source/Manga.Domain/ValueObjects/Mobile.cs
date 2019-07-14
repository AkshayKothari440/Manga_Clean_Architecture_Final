using System;
using System.Collections.Generic;
using System.Text;

namespace Manga.Domain.ValueObjects
{
   public class Mobile
    {
        private string _text;

        private Mobile() { }
        public Mobile(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new NameShouldNotBeEmptyException("The 'Mobile' field is required");

            _text = text;
        }

        public override string ToString()
        {
            return _text;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is string)
            {
                return obj.ToString() == _text;
            }

            return ((Mobile)obj)._text == _text;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + _text.GetHashCode();
                return hash;
            }
        }

        public bool Equals(Mobile other)
        {
            return this._text == other._text;
        }
    }
}
