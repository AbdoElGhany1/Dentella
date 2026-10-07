using Microsoft.AspNetCore.Identity;

namespace MainDentalla.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string Password { get; set; }
        public string Phone_number { get; set; }

        public int age { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        // Add other properties as needed
    }
}
