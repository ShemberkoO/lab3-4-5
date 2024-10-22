using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class Violation
{
    public int ViolationId { get; set; }

    public string Article { get; set; } = null!;

    public string? Comment { get; set; }

    public int? VictimId { get; set; }

    public virtual Victim? Victim { get; set; }
}
