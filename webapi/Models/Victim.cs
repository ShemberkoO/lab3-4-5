using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class Victim
{
    public int VictimId { get; set; }

    public string? PasportId { get; set; }

    public string Status { get; set; } = null!;

    public int? AccidentId { get; set; }

    public virtual Accident? Accident { get; set; }

    public virtual OccupantTransport? OccupantTransport { get; set; }

    public virtual Person? Pasport { get; set; }

    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
