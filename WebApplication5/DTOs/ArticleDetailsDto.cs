namespace MainDentalla.DTOs
{
    
        public class ArticleDetailsDto
        {
            public int ArticleId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string DoctorName { get; set; }
            public int NumberOfLikes { get; set; }
            public int NumberOfComments { get; set; }
            public string ImageUrl { get; set; }
            public DateTime CreatedAt { get; set; }
        // Add other properties as needed
    }

    
}
