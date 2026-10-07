using System.ComponentModel.DataAnnotations;

namespace MainDentalla.DTOs
{
    public class LoginDoctor
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string PassWord { get; set; }
    }
}
