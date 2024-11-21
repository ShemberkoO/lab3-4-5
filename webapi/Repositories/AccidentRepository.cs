using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using webapi.DTOs;
using webapi.Models;

namespace YourNamespace.Repositories
{
    public class AccidentRepository : IAccidentRepository
    {
        private static  DtpBdLabsContext _context;
        private static IDbContextTransaction current_transaction;

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

        public async Task<string> UpdateAccidentAsync(int id, AccidentDto accidentDto)
        {
            if (current_transaction != null) { 
            
            }
            if (id != accidentDto.AccidentId)
            {
                return "params not valid";
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
                return "changes saved";
            }
            catch (DbUpdateConcurrencyException err)
            {
                return "err: " + err.Message;  
            }
        }
        public async Task<string> StartTransactionAsync()
        {
            if (current_transaction != null)
            {
                try
                {
                    await current_transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    current_transaction.RollbackAsync();
                    current_transaction.Dispose();
                }
            }

            // Починаємо нову транзакцію з рівнем ізоляції SERIALIZABLE
            current_transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

            return "Transaction started.";
        }

        public async  Task<string> Commit()
        {
            if(current_transaction != null)
            {
                try
                {
                    await current_transaction.CommitAsync();
                    return "commited succesfully";
                }catch(Exception e)
                {
                    return "err: " + e.Message;
                }
                
            }
            else
            {
                return "no transaction";
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
        public async Task<string> DeleteAccidentsBeforeDateAsync(DeleteAccidentsInfo cutoffDate)
        {
            if (cutoffDate.Date == default)
            {
                return "Invalid date provided.";
            }

            try
            {
                
                var result = await _context
                    .Database
                    .SqlQuery<string>($"SELECT DeleteAccidentsBeforeDate({cutoffDate.Date})")
                    .ToListAsync();

                return result.FirstOrDefault() ?? "No result returned from the database.";
            }
            catch (Exception ex)
            {
     
                return $"Error deleting accidents: {ex.Message}";
            }
        }

        public async Task SimulateTransactionConflict()
        {
         

           PostgresTransactionConflictSimulator s = new PostgresTransactionConflictSimulator();
            s.SimulateConflict();
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

        public void setContext(DtpBdLabsContext context)
        {
            _context = context;
        }
    }
}
