using MainDentalla.Models;

namespace MainDentalla.DTOs
{
    public class CardDetails
    {
        public SpecialtyType Specialty { get; set; }
        public string DoctorName { get; set; }
        public byte[] DoctorPhoto { get; set; }
        public string Bio { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrentUniversity { get; set; }
        public ICollection<DoctorAvailabilityDto> DoctorAvailability { get; set; }
    }



}

