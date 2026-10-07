using Dentella3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MainDentalla.DTOs;
using MainDentalla.Models;
using Microsoft.AspNetCore.Authentication;

namespace MainDentalla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration confg;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Appdbcontext _context;

        public AccountController(UserManager<IdentityUser> userManager,IConfiguration Confg, RoleManager<IdentityRole> roleManager, Appdbcontext context)
        {
            _userManager = userManager;
            confg = Confg;
            _roleManager = roleManager;
            _context = context;
        }
        [HttpPost("patient/register")]
        public async Task<IActionResult> RegisterPatient([FromBody] PatientRegisterDto model)
        {
            // Check if the "Patient" role exists, if not, create it
            if (!await _roleManager.RoleExistsAsync("Patient"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Patient"));
            }

            // Continue with patient registration logic
            var user = new IdentityUser
            {
                
                UserName= model.username,
                Email = model.Email,
                PhoneNumber=model.phone_number
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Assign the "Patient" role to the user
                await _userManager.AddToRoleAsync(user, "Patient");

                // Create a new Patient entity with additional patient-specific data
                var patient = new Patient
                {
                    Name = model.username,
                    Email= model.Email,
                    Phone_number=model.phone_number,
                    //Id=int.Parse(user.Id),
                    //Password=model.Password
                   
                };

                // Save patient data to the database
                _context.Patients.Add(patient);
                _context.SaveChanges();

                // You may return additional information if needed
                return Ok();
            }

            return BadRequest(result.Errors);
        }
        [HttpPost("doctor/register")]
        public async Task<IActionResult> RegisterDoctor([FromBody] DoctorRegisterDto model)
        {
            // Check if the "Doctor" role exists, if not, create it
            if (!await _roleManager.RoleExistsAsync("Doctor"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Doctor"));
            }

            // Continue with doctor registration logic
            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Assign the "Doctor" role to the user
                await _userManager.AddToRoleAsync(user, "Doctor");

                // Create a new Doctor entity with additional doctor-specific data
                var doctor = new Doctor
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    //Id =int.Parse(user.Id),
                    //Password = model.Password,
                    DentistId=model.DentistID,
                    UserId=user.Id
                    
                    
                };

                // Save doctor data to the database
                _context.Doctors.Add(doctor);
                _context.SaveChanges();

                // You may return additional information if needed
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("patient/login")]
        public async Task<IActionResult> LoginPatient([FromBody] LoginPatient patient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityUser user = await _userManager.FindByNameAsync(patient.UserName);
                    if (user != null)
                    {
                        try
                        {
                            bool found = await _userManager.CheckPasswordAsync(user, patient.PassWord);
                            if (found)
                            {
                                var claims = new List<Claim>();
                                claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                                //GET ROLES
                                var roles = await _userManager.GetRolesAsync(user);
                                foreach (var itemRole in roles)
                                {
                                    claims.Add(new Claim(ClaimTypes.Role, itemRole));
                                }
                                SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(confg["JWT:Secret"]));
                                SigningCredentials signingCR = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                                JwtSecurityToken mytoken = new JwtSecurityToken(
                                    issuer: confg["JWT:VaildIssuer"],
                                    audience: confg["JWT:VaildAudiance"],
                                    claims: claims,
                                    expires: DateTime.Now.AddHours(1),
                                    signingCredentials: signingCR
                                    );
                                return Ok(new
                                {
                                    Token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                                    expiration = mytoken.ValidTo
                                });
                            }

                        }
                        catch (Exception ex) { return NotFound(); }
                    }
                    return NotFound();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                // Handle the exception when user is not found
                return NotFound();
            }
        }

        [HttpPost("doctor/login")]
        public async Task<IActionResult> LoginDoctor([FromBody] LoginDoctor doctor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityUser user = await _userManager.FindByEmailAsync(doctor.Email);
                    if (user != null)
                    {
                        try
                        {
                            bool found = await _userManager.CheckPasswordAsync(user, doctor.PassWord);
                            if (found)
                            {
                                var claims = new List<Claim>();
                                claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                                //GET ROLES
                                var roles = await _userManager.GetRolesAsync(user);
                                foreach (var itemRole in roles)
                                {
                                    claims.Add(new Claim(ClaimTypes.Role, itemRole));
                                }
                                SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(confg["JWT:Secret"]));
                                SigningCredentials signingCR = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                                JwtSecurityToken mytoken = new JwtSecurityToken(
                                    issuer: confg["JWT:VaildIssuer"],
                                    audience: confg["JWT:VaildAudiance"],
                                    claims: claims,
                                    expires: DateTime.Now.AddHours(1),
                                    signingCredentials: signingCR
                                    );
                                return Ok(new
                                {
                                    Token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                                    expiration = mytoken.ValidTo

                                });
                            }
                        }
                        catch (Exception ex) { return NotFound(); }
                    }
                    return NotFound();

                }
                return NotFound();
            }
            catch (Exception ex)
            {
                // Handle the exception when user is not found
                return NotFound();
            }
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            
            HttpContext.SignOutAsync();

            return Ok();
        }

    }

}

