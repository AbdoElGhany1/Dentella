using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainDentalla.DTOs
{
    public class DoctorRegisterDto
    {
        public string UserName { get; set; }
        public string DentistID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //[Compare("Password")]
        //public string Confired_Password { get; set; }
        public string PhoneNumber { get; set; }

        // Add other properties as needed
    }
}
