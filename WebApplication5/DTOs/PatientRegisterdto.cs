using System.ComponentModel.DataAnnotations;

namespace MainDentalla.DTOs
{
    public class PatientRegisterDto
    {
        public string Email { get; set; }
        public string username { get; set; }
        public string Password { get; set; }
        public string phone_number { get; set; }
        //[Compare("Password")]
        //public string Confired_Password { get; set; }
        // Add other properties as needed
    }

}
