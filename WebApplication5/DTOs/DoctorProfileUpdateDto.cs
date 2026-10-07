namespace MainDentalla.DTOs
{
    public class DoctorProfileUpdateDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Bio { get; set; }
        public string Currentlevel { get; set; }
        public string CurrentUniversity { get; set; }
        public IFormFile Photo { get; set; }
    }

}
