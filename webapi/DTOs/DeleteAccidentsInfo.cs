using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.ComponentModel.DataAnnotations;
using webapi.Models;

namespace webapi.DTOs
{
    public class DeleteAccidentsInfo
    {
      
        [Required(ErrorMessage = "Date is required.")]
        public DateOnly Date { get; set; }

    }

}
