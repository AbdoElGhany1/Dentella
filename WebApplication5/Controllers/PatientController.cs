using Dentella3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MainDentalla.Models;

namespace MainDentalla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly Appdbcontext db;
        public PatientController(Appdbcontext _db)
        {
            this.db = _db;
        }
        
       
    }
}
