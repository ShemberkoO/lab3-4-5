using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class Accident
{
    public int AccidentId { get; set; }

    public DateOnly Date { get; set; }

    public string Location { get; set; } = null!;


    public string? Description { get; set; }

    public virtual ICollection<Victim> Victims { get; set; } = new List<Victim>();
}
