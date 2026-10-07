using System.ComponentModel.DataAnnotations;

namespace MainDentalla.DTOs
{
    public class LoginPatient
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string PassWord { get; set; }
    }
}
