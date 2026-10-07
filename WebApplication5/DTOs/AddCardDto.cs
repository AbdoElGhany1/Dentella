using MainDentalla.Models;

namespace MainDentalla.DTOs
{
    public class AddCardDto
    {
        public SpecialtyType Specialty { get; set; } // Example: "Cardiology"
        public DoctorAvailabilityDto DoctorAvailability { get; set; }
    }

    public class DoctorAvailabilityDto
    {
        public List<DateTime> AvailableDates { get; set; } // List of specific dates when the doctor is available
    }
}
