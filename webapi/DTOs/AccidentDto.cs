using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.ComponentModel.DataAnnotations;
using webapi.Models;

namespace webapi.DTOs
{
    public class AccidentDto
    {
        public int AccidentId { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DateAfter1500(ErrorMessage = "Date must be after the year 1500.")]
        public DateOnly Date { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Location must be between 3 and 100 characters.")]
        public string Location { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Description can't exceed 500 characters.")]
        public string? Description { get; set; }

        public ICollection<PersonDto> People { get; set; } = new List<PersonDto>();

        public ICollection<ViolationDto> Violations { get; set; } = new List<ViolationDto>();
    }

    public class DateAfter1500Attribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateOnly date)
            {
                if (date.Year <= 1500)
                {
                    return new ValidationResult(ErrorMessage ?? "Date must be after the year 1500.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
