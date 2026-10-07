using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MainDentalla.Models
{
    public class Doctor
    {

        public string DentistId { get; set; }
        public string UserName { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        //public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Bio { get; set; }
        public string? Currentlevel { get; set; }
        public string? CurrentUniversity { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public byte[]? Photo { get; set; } // Byte array to store the photo data
       
        // public virtual List<Article> Articles { get; set; }
        // Add other properties as needed
    }
}
