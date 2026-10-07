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
    public class DoctorController : ControllerBase
    {
        private readonly Appdbcontext _context;

        private readonly UserManager<IdentityUser> _userManager;

        public DoctorController(Appdbcontext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet("ReturnProfile")]
        [Authorize]
        public async Task<IActionResult> ReturnProfile()
        {
            // Retrieve user ID from JWT token claims
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            // Retrieve doctor details from the database based on user ID
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                //doctor.Id,
                doctor.UserName,
                doctor.Email,
                doctor.PhoneNumber,
                doctor.Bio,
                doctor.Currentlevel,
                doctor.CurrentUniversity,
                Photo = doctor.Photo != null ? Convert.ToBase64String(doctor.Photo) : null,
               
            });
        }

        [HttpPut("UpdateProfile")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateDoctorProfile([FromForm]DoctorProfileUpdateDto profileDto)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (doctor == null)
            {
                return NotFound();
            }

            // Update doctor profile
            doctor.UserName = profileDto.UserName;
            doctor.Email = profileDto.Email;
            doctor.PhoneNumber = profileDto.PhoneNumber;
            doctor.Bio = profileDto.Bio;
            doctor.Currentlevel = profileDto.Currentlevel;
            doctor.CurrentUniversity = profileDto.CurrentUniversity;
            if (profileDto.Photo != null && profileDto.Photo.Length > 0)
            {
                // Convert the uploaded photo to a byte array and save it
                using (var memoryStream = new MemoryStream())
                {
                    await profileDto.Photo.CopyToAsync(memoryStream);
                    doctor.Photo = memoryStream.ToArray();   
                }
            }

            _context.Update(doctor);
            await _context.SaveChangesAsync();

            return Ok();
        }


    }
} 
