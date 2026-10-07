using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainDentalla.Models
{
    public class Card
    {
        public int CardId { get; set; }

        // Constants
        public SpecialtyType Specialty { get; set; }

        // Availability from the calendar
        public ICollection<CalendarDay> CalendarDays { get; set; }

        public byte[]? DoctorPhoto { get; set; }
        public int? DoctorId { get; set; }
        public Doctor Doctor { get; set; } 
        

    }
    public enum SpecialtyType
    {
        Cleaning,
        Filling,
        Crowns,
        Implants,
        Extraction,
        Orthodontic
        // Add more specialties as needed
    }
}
