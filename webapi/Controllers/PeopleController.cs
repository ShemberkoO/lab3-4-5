using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.DTOs; // Import the DTOs namespace
using webapi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

[Route("api/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly DtpBdLabsContext _context;

    public PersonController(DtpBdLabsContext context)
    {
        _context = context;
    }

    // GET: api/Person
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonDto>>> GetPeople()
    {
        var people = await _context.People.ToListAsync();
        var peopleDto = people.Select(person => new PersonDto
        {
            PasportId = person.PasportId,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Patronymic = person.Patronymic,
            RegistrationAddress = person.RegistrationAddress
        }).ToList();

        return Ok(peopleDto);
    }
    // GET: api/Person/5
[HttpGet("{id}/show")]
    public async Task<ActionResult<PersonDto>> GetPerson(string id)
    {
       
        var person = await _context.People.FindAsync(id);
        if (person == null)
        {
            return NotFound();
        }

        var victims = await _context.Victims
            .Where(v => v.PasportId == person.PasportId)
            .ToListAsync();
    
        var accidentIds = victims.Select(v => v.AccidentId).ToList();
        var victimsIds = victims.Select(v => v.VictimId).ToList();

        var violations = await _context.Violations
            .Where(v => victimsIds.Contains(v.ViolationId)).Select(v=> new ViolationDto 
            {
                Article = v.Article,
                Comment = v.Comment,
                VictimId = v.VictimId,
                ViolationId = v.ViolationId
            })
            .ToListAsync();

        var accidents = await _context.Accidents
            .Where(a => accidentIds.Contains(a.AccidentId)).Select(a=> new AccidentDto {
                AccidentId = a.AccidentId,
                Date = a.Date,
                Location = a.Location,
                Description = a.Description
            })
            .ToListAsync();

        var personDto = new PersonDto
        {
            PasportId = person.PasportId,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Patronymic = person.Patronymic,
            RegistrationAddress = person.RegistrationAddress,
            Violations = violations, 
            Accidents = accidents 
        };

        return Ok(personDto);
    }


    // PUT: api/Person/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPerson(string id, PersonDto personDto)
    {
        if (id != personDto.PasportId)
        {
            return BadRequest();
        }

        // Map DTO to entity
        var person = new Person
        {
            PasportId = personDto.PasportId,
            FirstName = personDto.FirstName,
            LastName = personDto.LastName,
            Patronymic = personDto.Patronymic,
            RegistrationAddress = personDto.RegistrationAddress
        };

        _context.Entry(person).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PersonExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Person
    [HttpPost]
    public async Task<ActionResult<PersonDto>> PostPerson(PersonDto personDto)
    {
        // Map DTO to entity
        var person = new Person
        {
            PasportId = personDto.PasportId,
            FirstName = personDto.FirstName,
            LastName = personDto.LastName,
            Patronymic = personDto.Patronymic,
            RegistrationAddress = personDto.RegistrationAddress
        };

        _context.People.Add(person);
        await _context.SaveChangesAsync();

        var createdPersonDto = new PersonDto
        {
            PasportId = person.PasportId,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Patronymic = person.Patronymic,
            RegistrationAddress = person.RegistrationAddress
        };

        return CreatedAtAction("GetPerson", new { id = person.PasportId }, createdPersonDto);
    }


    // DELETE: api/Person/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(string id)
    {
        var person = await _context.People.FindAsync(id);
        if (person == null)
        {
            return NotFound();
        }

        _context.People.Remove(person);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PersonExists(string id)
    {
        return _context.People.Any(e => e.PasportId == id);
    }
}
