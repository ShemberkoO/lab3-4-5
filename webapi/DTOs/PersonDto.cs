using System.ComponentModel.DataAnnotations;
using webapi.Models;

namespace webapi.DTOs
{
    public class PersonDto
    {
        [Required(ErrorMessage = "Passport ID is required.")]
        [StringLength(20, ErrorMessage = "Passport ID cannot be longer than 20 characters.")]
        public string PasportId { get; set; } = null!;

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        public string LastName { get; set; } = null!;

        [StringLength(50, ErrorMessage = "Patronymic cannot be longer than 50 characters.")]
        public string? Patronymic { get; set; }

        [Required(ErrorMessage = "Registration address is required.")]
        [StringLength(100, ErrorMessage = "Registration address cannot be longer than 100 characters.")]
        public string RegistrationAddress { get; set; } = null!;


        public ICollection<AccidentDto> Accidents { get; set; } = new List<AccidentDto>();
        public ICollection<ViolationDto> Violations { get; set; } = new List<ViolationDto>();

    }
}
