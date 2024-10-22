using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.DTOs;

namespace YourNamespace.Repositories
{
    public interface IPersonRepository
    {
        Task<IEnumerable<PersonDto>> GetPeopleAsync();
        Task<PersonDto> GetPersonByIdAsync(string id);
        Task AddPersonAsync(PersonDto personDto);
        Task<bool> UpdatePersonAsync(string id, PersonDto personDto);
        Task<bool> DeletePersonAsync(string id);
        Task<bool> PersonExistsAsync(string id);
    }
}
