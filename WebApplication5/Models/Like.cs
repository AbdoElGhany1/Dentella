using Microsoft.AspNetCore.Identity;

namespace MainDentalla.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public string? UserId { get; set; }
        public virtual IdentityUser User { get; set; }
        public Article Article { get; set; }
    }
}
