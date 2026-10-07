using MainDentalla.Models;

namespace MainDentalla.DTOs
{
    public class CardDto
    {
        public int CardId { get; set; }
        public SpecialtyType Specialty { get; set; }
        public string DoctorName { get; set; }
        public byte[] DoctorPhoto { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrentUniversity { get; set; }
    }

}
