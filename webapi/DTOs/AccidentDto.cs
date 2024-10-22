using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.ComponentModel.DataAnnotations;
using webapi.Models;

namespace webapi.DTOs
{
    public class AccidentDto
    {
        [Required(ErrorMessage = "Required.")]
        public int AccidentId { get; set; }

        [Required(ErrorMessage = "Required.")]
        public DateOnly Date { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Location { get; set; } = null!;
        public string? Description { get; set; }

        public ICollection<PersonDto> People { get; set; } = new List<PersonDto>();
        public ICollection<ViolationDto> Violations { get; set; } = new List<ViolationDto>();

    }
}
