using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class AccidentAnalysis
{
    public int? AccidentId { get; set; }

    public string? Location { get; set; }

    public DateOnly? Date { get; set; }

    public long? VictimCount { get; set; }
}
