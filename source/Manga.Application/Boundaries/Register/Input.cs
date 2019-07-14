namespace Manga.Application.Boundaries.Register
{
    using Manga.Domain.ValueObjects;

    public sealed class Input
    {
        public SSN SSN { get; }
        public Name Name { get; }
        public Email Email { get; }
        public Mobile Mobile { get; }
        public PositiveAmount InitialAmount { get; }

        public Password Password { get; } //add
        public Input(SSN ssn, Name name, Email email,Mobile mobile, Password password, PositiveAmount initialAmount)
        {
            SSN = ssn;
            Name = name;
            Email = email;
            Mobile = mobile;
            InitialAmount = initialAmount;
            Password = password;
        }
    }
}