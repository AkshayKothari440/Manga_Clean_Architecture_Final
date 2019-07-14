namespace Manga.WebApi.UseCases.Register
{
    public class RegisterRequest
    {
        public string SSN { get; set; }
        public string Name { get; set; }
        public double InitialAmount { get; set; }
        public string Email { get; set; } 
        public string Mobile { get; set; }
        
        public string Password { get; set; } // add

    }
}