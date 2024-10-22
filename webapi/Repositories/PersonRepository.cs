using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.DTOs;
using webapi.Models;

namespace YourNamespace.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DtpBdLabsContext _context;

        public PersonRepository(DtpBdLabsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PersonDto>> GetPeopleAsync()
        {
            var people = await _context.People.ToListAsync();
            return people.Select(person => new PersonDto
            {
                PasportId = person.PasportId,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Patronymic = person.Patronymic,
                RegistrationAddress = person.RegistrationAddress
            }).ToList();
        }

        public async Task<PersonDto> GetPersonByIdAsync(string id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null) return null;

            var victims = await _context.Victims
                .Where(v => v.PasportId == person.PasportId)
                .ToListAsync();
            var accidentIds = victims.Select(v => v.AccidentId).ToList();
            var victimsIds = victims.Select(v => v.VictimId).ToList();

            var violations = await _context.Violations
                .Where(v => victimsIds.Contains(v.ViolationId))
                .Select(v => new ViolationDto
                {
                    Article = v.Article,
                    Comment = v.Comment,
                    VictimId = v.VictimId,
                    ViolationId = v.ViolationId
                })
                .ToListAsync();

            var accidents = await _context.Accidents
                .Where(a => accidentIds.Contains(a.AccidentId))
                .Select(a => new AccidentDto
                {
                    AccidentId = a.AccidentId,
                    Date = a.Date,
                    Location = a.Location,
                    Description = a.Description
                })
                .ToListAsync();

            return new PersonDto
            {
                PasportId = person.PasportId,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Patronymic = person.Patronymic,
                RegistrationAddress = person.RegistrationAddress,
                Violations = violations,
                Accidents = accidents
            };
        }

        public async Task AddPersonAsync(PersonDto personDto)
        {
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
        }

        public async Task<bool> UpdatePersonAsync(string id, PersonDto personDto)
        {
            if (id != personDto.PasportId) return false;

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
                if (!await PersonExistsAsync(id)) return false;
                throw;
            }
            return true;
        }

        public async Task<bool> DeletePersonAsync(string id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null) return false;

            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PersonExistsAsync(string id)
        {
            return await _context.People.AnyAsync(e => e.PasportId == id);
        }
    }
}
