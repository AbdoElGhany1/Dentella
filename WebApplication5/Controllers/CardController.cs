using Dentella3.Models;
using MainDentalla.DTOs;
using MainDentalla.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MainDentalla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly Appdbcontext _context;

        private readonly UserManager<IdentityUser> _userManager;

        public CardController(Appdbcontext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("AddCard")]
        [Authorize(Roles ="Doctor")]
        public async Task<IActionResult> AddCard([FromBody] AddCardDto model)
        {
            // Retrieve the doctor's ID from the JWT token
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var doctor = await _userManager.FindByIdAsync(userId);

            if (doctor == null)
            {
                return NotFound("Doctor not found");
            }
            var doctorId = _context.Doctors.FirstOrDefaultAsync(e => e.UserId == userId);
            var card = new Card
            {
                Specialty = model.Specialty,
                Doctor = await doctorId,
                // Set other properties as needed

                // Initialize CalendarDays based on DoctorAvailability
                CalendarDays = model.DoctorAvailability?.AvailableDates?.Select(date => new CalendarDay
                {
                    Date = date,
                    // You can set additional properties for CalendarDay if needed
                }).ToList()
            };


            // Save the Card to the database
            _context.Cards.Add(card);
            _context.SaveChanges();

            return Ok("Card added successfully");
        }
        [HttpGet("GetAllCards")]
        public async Task<IActionResult> GetAllCards()
        {
            var cardDetails = await _context.Cards
                .Include(a => a.Doctor)
                .ToListAsync();
            
            var carddetailsDtos = cardDetails
                .Select(card => new CardDto
                {      
                    CardId = card.CardId,
                    Specialty= card.Specialty,
                    DoctorName = card.Doctor.UserName,
                    PhoneNumber = card.Doctor.PhoneNumber,
                    CurrentUniversity=card.Doctor.CurrentUniversity,
                    DoctorPhoto=card.Doctor.Photo
                })
                .ToList();

            return Ok(carddetailsDtos);
        }
        [HttpGet("GetSpecsificCards/{specialty}")]
        public async Task<IActionResult> GetSpecificCard(SpecialtyType specialty)
        {
            try
            {
                var cardDetails = await _context.Cards
                    .Include(a => a.Doctor)
                    .Where(card => card.Specialty == specialty)
                    .ToListAsync();

                var carddetailsDtos = cardDetails
                    .Select(card => new CardDto
                    {
                        CardId = card.CardId,
                        Specialty = card.Specialty,
                        DoctorName = card.Doctor.UserName,
                        PhoneNumber = card.Doctor.PhoneNumber,
                        CurrentUniversity = card.Doctor.CurrentUniversity,
                        //DoctorImage = card.Doctor.,
                    })
                    .ToList();

                return Ok(carddetailsDtos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        [HttpDelete("RemoveCard/{CardId}")]
        [Authorize]
        public IActionResult RemoveCard(int CardId)
        {

            // Retrieve the user ID from the JWT token claims
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID not found in the JWT token");
            }

            // Check if the comment exists and is associated with the user
            var Car = _context.Cards.FirstOrDefault(c => c.CardId == CardId && c.Doctor.UserId == userId);

            if (Car == null)
            {
                return NotFound("Card not found or you don't have permission to remove it");
            }

            
            _context.Cards.Remove(Car);
            _context.SaveChanges();

            return Ok("Card removed successfully");
        }

        [HttpGet("GetCardDetails/{cardid}")]
        public async Task<IActionResult> GetCardDetails(int cardid)
        {
            try
            {
                var cardDetails = await _context.Cards
                    .Include(a => a.Doctor)
                    .Include(a => a.CalendarDays)
                    .Where(card => card.CardId == cardid)
                    .FirstOrDefaultAsync();

                if (cardDetails == null)
                {
                    return NotFound($"Card with ID {cardid} not found");
                }

                var cardDetailsDto = new CardDetails
                {
                    
                    Specialty = cardDetails.Specialty,
                    DoctorName = cardDetails.Doctor.UserName,
                    DoctorPhoto=cardDetails.Doctor.Photo,
                    PhoneNumber = cardDetails.Doctor.PhoneNumber,
                    CurrentUniversity = cardDetails.Doctor.CurrentUniversity,
                    Bio = cardDetails.Doctor.Bio,
                    DoctorAvailability = new List<DoctorAvailabilityDto>
                    {
                        new DoctorAvailabilityDto
                        {
                            AvailableDates = cardDetails.CalendarDays
                                .Select(calendarDay => calendarDay.Date)
                                .ToList()
                        }
                    }
                };

                return Ok(cardDetailsDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("SearchCardsByUniversity/{university}")]
        public IActionResult SearchCardsByUniversity(string university)
        {
            try
            {
                // Use LINQ to search for cards with the specified university
                var matchingCards = _context.Cards
                    .Include(card => card.Doctor) // Include Doctor for CurrentUniversity
                    .Where(card => card.Doctor.CurrentUniversity == university)
                    .ToList();

                if (matchingCards.Count == 0)
                {
                    return NotFound($"No cards found for university: {university}");
                }

                // Map the result to a DTO or another appropriate model if needed
                var cardDtos = matchingCards.Select(card => new CardDto
                {
                    CardId = card.CardId,
                    Specialty = card.Specialty,
                    DoctorName = card.Doctor.UserName,
                    PhoneNumber = card.Doctor.PhoneNumber,
                    CurrentUniversity = card.Doctor.CurrentUniversity,
                    // Add other properties as needed
                }).ToList();

                return Ok(cardDtos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

    }

}






