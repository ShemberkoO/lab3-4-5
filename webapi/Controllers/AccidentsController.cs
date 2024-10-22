//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using webapi.DTOs; // Add the namespace for the DTOs
//using webapi.Models;

//namespace YourNamespace.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AccidentsController : ControllerBase
//    {
//        private readonly DtpBdLabsContext _context;

//        public AccidentsController(DtpBdLabsContext context)
//        {
//            _context = context;
//        }

//        // GET: api/Accidents
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<AccidentDto>>> GetAccidents()
//        {
//            var accidents = await _context.Accidents.ToListAsync();
//            var accidentsDto = accidents.Select(accident => new AccidentDto
//            {
//                AccidentId = accident.AccidentId,
//                Date = accident.Date,
//                Location = accident.Location,
//                Description = accident.Description
//            }).ToList();

//            return Ok(accidentsDto);
//        }

//        // GET: api/Accidents/5
//        [HttpGet("{id}/show")]
//        public async Task<ActionResult<AccidentDto>> GetAccident(int id)
//        {

//            var accident = await _context.Accidents.FindAsync(id);
//            if (accident == null)
//            {
//                return NotFound();
//            }
//            // Отримати список всіх жертв, пов'язаних з нещасним випадком
//            var victims = await _context.Victims
//                .Where(v => v.AccidentId == id)
//                .ToListAsync();

//            // Отримати ідентифікатори жертв для отримання порушень
//            var victimIds = victims.Select(v => v.VictimId).ToList();

//            // Отримати пов'язані порушення через жертв
//            var violations = await _context.Violations
//                .Where(v => victimIds.Contains(v.ViolationId))
//                .Select(v => new ViolationDto
//                {
//                    ViolationId = v.ViolationId,
//                    Article = v.Article,
//                    Comment = v.Comment,
//                    VictimId = v.VictimId
//                })
//                .ToListAsync();

//            var peopleIds  = victims.Select(v => v.PasportId).ToList();


//            var people = await _context.People
//                .Where(p => peopleIds.Contains(p.PasportId))
//                .Select(v => new PersonDto
//                {
//                    PasportId = v.PasportId,
//                    FirstName = v.FirstName,
//                    LastName = v.LastName,
//                    Patronymic = v.Patronymic,
//                    RegistrationAddress = v.RegistrationAddress
//                })
//                .ToListAsync();


//            // Створити DTO для повернення даних
//            var accidentDto = new AccidentDto
//            {
//                AccidentId = accident.AccidentId,
//                Date = accident.Date,
//                Location = accident.Location,
//                Description = accident.Description,
//                Violations = violations, 
//                People = people
//            };

//            return Ok(accidentDto);
//        }


//        // POST: api/Accidents
//        [HttpPost]
//        public async Task<ActionResult<AccidentDto>> PostAccident(AccidentDto accidentDto)
//        {
//            var accident = new Accident
//            {
//                Date = accidentDto.Date,
//                Location = accidentDto.Location,
//                Description = accidentDto.Description
//            };

//            _context.Accidents.Add(accident);
//            await _context.SaveChangesAsync();

//            accidentDto.AccidentId = accident.AccidentId;

//            return CreatedAtAction(nameof(GetAccident), new { id = accident.AccidentId }, accidentDto);
//        }

//        // PUT: api/Accidents/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutAccident(int id, AccidentDto accidentDto)
//        {
//            if (id != accidentDto.AccidentId)
//            {
//                return BadRequest();
//            }

//            var accident = new Accident
//            {
//                AccidentId = accidentDto.AccidentId,
//                Date = accidentDto.Date,
//                Location = accidentDto.Location,
//                Description = accidentDto.Description
//            };

//            _context.Entry(accident).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!AccidentExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // DELETE: api/Accidents/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteAccident(int id)
//        {
//            var accident = await _context.Accidents.FindAsync(id);
//            if (accident == null)
//            {
//                return NotFound();
//            }

//            _context.Accidents.Remove(accident);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool AccidentExists(int id)
//        {
//            return _context.Accidents.Any(e => e.AccidentId == id);
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.DTOs;
using YourNamespace.Repositories;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccidentsController : ControllerBase
    {
        private readonly IAccidentRepository _accidentRepository;

        public AccidentsController(IAccidentRepository accidentRepository)
        {
            _accidentRepository = accidentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccidentDto>>> GetAccidents()
        {
            var accidents = await _accidentRepository.GetAccidentsAsync();
            return Ok(accidents);
        }

        [HttpGet("{id}/show")]
        public async Task<ActionResult<AccidentDto>> GetAccident(int id)
        {
            var accident = await _accidentRepository.GetAccidentByIdAsync(id);
            if (accident == null)
            {
                return NotFound();
            }
            return Ok(accident);
        }

        // POST: api/Accidents
        [HttpPost]
        public async Task<ActionResult<AccidentDto>> PostAccident(AccidentDto accidentDto)
        {
            var accident = await _accidentRepository.CreateAccidentAsync(accidentDto);
            accidentDto.AccidentId = accident.AccidentId;
            return CreatedAtAction(nameof(GetAccident), new { id = accident.AccidentId }, accidentDto);
        }

        // PUT: api/Accidents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccident(int id, AccidentDto accidentDto)
        {
            var result = await _accidentRepository.UpdateAccidentAsync(id, accidentDto);
            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // DELETE: api/Accidents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccident(int id)
        {
            var result = await _accidentRepository.DeleteAccidentAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
