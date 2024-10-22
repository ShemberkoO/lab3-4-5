using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using webapi.Models;

namespace webapi.DTOs;

public partial class VictimDto
{
    [Required(ErrorMessage = "Required.")]
    public int VictimId { get; set; }

    [Required(ErrorMessage = "Required.")]
    public string? PasportId { get; set; }


    public string Status { get; set; } = null!;

    [Required(ErrorMessage = "Required.")]
    public int? AccidentId { get; set; }

    public virtual Accident? Accident { get; set; }

    public virtual OccupantTransport? OccupantTransport { get; set; }

    public virtual Person? Pasport { get; set; }

}
