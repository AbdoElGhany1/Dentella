namespace MainDentalla.Models
{
    public class CalendarDay
    {
        public int CalendarDayId { get; set; }

        public DateTime Date { get; set; }

        public int CardId { get; set; }
        public Card Card { get; set; } // Navigation property to Card model
    }
}
