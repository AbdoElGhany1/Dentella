using Dentella3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MainDentalla.DTOs;
using MainDentalla.Models;

namespace MainDentalla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ArticleController : ControllerBase
    {
        private readonly Appdbcontext _context;

        private readonly UserManager<IdentityUser> _userManager;

        public ArticleController(Appdbcontext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost("AddArticles")]
        [Authorize] // Requires authentication
        public async Task<IActionResult> AddArticle([FromForm] ArticleDto articleDto)
        {
            // Retrieve the doctor's ID from the JWT token
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var doctor = await _userManager.FindByIdAsync(userId);

            if (doctor == null)
            {
                return NotFound("Doctor not found");
            }

            var doctorId = _context.Doctors.FirstOrDefaultAsync(e => e.UserId == userId);

            var article = new Article
            {
                Title = articleDto.Title,
                Content = articleDto.Content,
                CreatedAt = DateTime.UtcNow,
                Doctor = await doctorId,
                
            };

           // if (articleDto.ImageData != null && articleDto.ImageData.Length > 0)
           // {
                // Convert the uploaded photo to a byte array and save it
                using (var memoryStream = new MemoryStream())
                {
                    await articleDto.ImageData.CopyToAsync(memoryStream);
                    article.ImageData = memoryStream.ToArray();
                }
            //}
           

            _context.Articles.Add(article);
            _context.SaveChanges();

            return Ok();
        }




        [HttpGet("GetAllArticles")]
        public async Task<IActionResult> GetArticleDetails()
        {
            var articlesDetails = await _context.Articles
                .Include(a => a.Doctor)
                .ToListAsync();

            var articleDetailsDtos = articlesDetails
                .Select(article => new ArticleDetailsDto
                {
                    ArticleId = article.Id,
                    Title = article.Title,
                    Content = article.Content,
                    CreatedAt= DateTime.UtcNow,
                    DoctorName = article.Doctor.UserName,
                    NumberOfLikes = _context.Likes.Count(like => like.ArticleId == article.Id),
                    NumberOfComments = _context.Comments.Count(comment => comment.ArticleId == article.Id),
                   
                })
                .ToList();

            return Ok(articleDetailsDtos);
        }

        [HttpPost("LikeTheArticle/{articleId}")]
        [Authorize]
        public IActionResult AddLikeToArticle(int articleId)
        {
            var article = _context.Articles.Find(articleId);

            if (article == null)
            {
                return NotFound();
            }

            // Retrieve the user ID from the JWT token claims
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            // Check if the user has already liked the article
            var existingLike = _context.Likes.FirstOrDefault(l => l.ArticleId == articleId && l.UserId == userId);

            if (existingLike != null)
            {
                // Remove the existing like
                _context.Likes.Remove(existingLike);
                _context.SaveChanges();

                return Ok();
            }

            // Create a new like
            var newLike = new Like
            {
                ArticleId = articleId,
                UserId = userId 
                               
            };

            _context.Likes.Add(newLike);
            _context.SaveChanges();

            return Ok();
        }


        [HttpPost("CommentOnArticle/{articleId}")]
        [Authorize] 
        public IActionResult AddCommentToArticle(int articleId, [FromBody] CommentDto commentDto)
        {
            var article = _context.Articles.Find(articleId);

            if (article == null)
            {
                return NotFound();
            }

            // Retrieve the user ID from the JWT token claims
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            // Create a new comment
            var newComment = new Comment
            {
                ArticleId = articleId,
                UserId = userId,
                CreatedAt= DateTime.UtcNow,
                Content = commentDto.Content
                
            };

            _context.Comments.Add(newComment);
            _context.SaveChanges();

            return Ok();
        }


        [HttpDelete("RemoveComment/{articleId}/{commentId}")]
        [Authorize] 
        public IActionResult RemoveCommentFromArticle(int articleId, int commentId)
        {
            var article = _context.Articles.Find(articleId);

            if (article == null)
            {
                return NotFound("Article not found");
            }

            // Retrieve the user ID from the JWT token claims
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID not found in the JWT token");
            }

            // Check if the comment exists and is associated with the user
            var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId && c.ArticleId == articleId && c.UserId == userId);

            if (comment == null)
            {
                return NotFound("Comment not found or you don't have permission to remove it");
            }

            // Remove the comment
            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return Ok("Comment removed successfully");
        }


        [HttpGet("GetCommentsForOneArticle/{articleId}")]
        public IActionResult GetCommentsForArticle(int articleId)
        {
            var article = _context.Articles.Find(articleId);

            if (article == null)
            {
                return NotFound("Article not found");
            }

            var comments = _context.Comments
                .Where(c => c.ArticleId == articleId)
                .Select(comment => new CommentList
                {
                    UserName = comment.User.UserName,
                    Content = comment.Content,
                    CreatedAt = comment.CreatedAt
                })
                .ToList();

            return Ok(comments);
        }
        [HttpDelete("RemoveArticle/{articleId}")]
        [Authorize]
        public IActionResult RemoveArticle(int articleId)
        {
           
            // Retrieve the user ID from the JWT token claims
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID not found in the JWT token");
            }

            // Check if the comment exists and is associated with the user
            var article1 = _context.Articles.FirstOrDefault(c => c.Id == articleId && c.Doctor.UserId == userId);

            if (article1 == null)
            {
                return NotFound("Article not found or you don't have permission to remove it");
            }

            // Remove the comment
            _context.Articles.Remove(article1);
            _context.SaveChanges();

            return Ok("Article removed successfully");
        }
    }
}
