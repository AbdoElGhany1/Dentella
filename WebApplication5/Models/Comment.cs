using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace MainDentalla.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        [JsonIgnore]
        public Article Article { get; set; }
        public string? UserId { get; set; }
        public IdentityUser User { get; set; }  
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
