using Microsoft.AspNetCore.Identity;

namespace MainDentalla.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        //public string ImageUrl { get; set; } // URL or path to the image
        public byte[]? ImageData { get; set; } // Changed to byte[]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }

}
