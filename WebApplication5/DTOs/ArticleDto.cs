namespace MainDentalla.DTOs
{
    public class ArticleDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
      //  public string ImageUrl { get; set; }
        public IFormFile ImageData { get; set; }
    }

}
