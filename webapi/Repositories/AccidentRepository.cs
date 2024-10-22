using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.DTOs;
using webapi.Models;

namespace YourNamespace.Repositories
{
    public class AccidentRepository : IAccidentRepository
    {
        private readonly DtpBdLabsContext _context;

        public AccidentRepository(DtpBdLabsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AccidentDto>> GetAccidentsAsync()
        {
            return await _context.Accidents
                .Select(accident => new AccidentDto
                {
                    AccidentId = accident.AccidentId,
                    Date = accident.Date,
                    Location = accident.Location,
                    Description = accident.Description
                })
                .ToListAsync();
        }

        public async Task<AccidentDto> GetAccidentByIdAsync(int id)
        {
            var accident = await _context.Accidents.FindAsync(id);
            if (accident == null)
            {
                return null;
            }

            var victims = await GetVictimsByAccidentIdAsync(id);
            var victimIds = victims.Select(v => v.VictimId).ToList();
            var violations = await GetViolationsByVictimsAsync(victimIds);
            var personIds = victims.Select(v => v.PasportId).ToList();
            var people = await GetPeopleByVictimIdsAsync(personIds);

            return new AccidentDto
            {
                AccidentId = accident.AccidentId,
                Date = accident.Date,
                Location = accident.Location,
                Description = accident.Description,
                Violations = violations,
                People = people
            };
        }

        public async Task<Accident> CreateAccidentAsync(AccidentDto accidentDto)
        {
            var accident = new Accident
            {
                Date = accidentDto.Date,
                Location = accidentDto.Location,
                Description = accidentDto.Description
            };

            _context.Accidents.Add(accident);
            await _context.SaveChangesAsync();
            return accident;
        }

        public async Task<bool> UpdateAccidentAsync(int id, AccidentDto accidentDto)
        {
            if (id != accidentDto.AccidentId)
            {
                return false;
            }

            var accident = new Accident
            {
                AccidentId = accidentDto.AccidentId,
                Date = accidentDto.Date,
                Location = accidentDto.Location,
                Description = accidentDto.Description
            };

            _context.Entry(accident).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccidentExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteAccidentAsync(int id)
        {
            var accident = await _context.Accidents.FindAsync(id);
            if (accident == null)
            {
                return false;
            }

            _context.Accidents.Remove(accident);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Victim>> GetVictimsByAccidentIdAsync(int accidentId)
        {
            return await _context.Victims
                .Where(v => v.AccidentId == accidentId)
                .ToListAsync();
        }

        public async Task<List<ViolationDto>> GetViolationsByVictimsAsync(List<int> victimIds)
        {
            return await _context.Violations
                .Where(v => victimIds.Contains(v.ViolationId))
                .Select(v => new ViolationDto
                {
                    ViolationId = v.ViolationId,
                    Article = v.Article,
                    Comment = v.Comment,
                    VictimId = v.VictimId
                })
                .ToListAsync();
        }

        public async Task<List<PersonDto>> GetPeopleByVictimIdsAsync(List<string> personIds)
        {
            return await _context.People
                .Where(p => personIds.Contains(p.PasportId))
                .Select(p => new PersonDto
                {
                    PasportId = p.PasportId,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Patronymic = p.Patronymic,
                    RegistrationAddress = p.RegistrationAddress
                })
                .ToListAsync();
        }

        public bool AccidentExists(int id)
        {
            return _context.Accidents.Any(e => e.AccidentId == id);
        }
    }
}
