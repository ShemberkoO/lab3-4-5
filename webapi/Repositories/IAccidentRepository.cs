using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.DTOs;
using webapi.Models;

namespace YourNamespace.Repositories
{
    public interface IAccidentRepository
    {
        Task<IEnumerable<AccidentDto>> GetAccidentsAsync();
        Task<AccidentDto> GetAccidentByIdAsync(int id);
        Task<Accident> CreateAccidentAsync(AccidentDto accidentDto);
        Task<bool> UpdateAccidentAsync(int id, AccidentDto accidentDto);
        Task<bool> DeleteAccidentAsync(int id);
        Task<List<Victim>> GetVictimsByAccidentIdAsync(int accidentId);
        Task<List<ViolationDto>> GetViolationsByVictimsAsync(List<int> victimIds);
        Task<List<PersonDto>> GetPeopleByVictimIdsAsync(List<string> personIds);
        bool AccidentExists(int id);
    }
}
