using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class AccidentAnalysisWithArticle
{
    public int? AccidentId { get; set; }

    public string? Location { get; set; }

    public DateOnly? Date { get; set; }

    public long? VictimCount { get; set; }

    public long? ViolationsNum { get; set; }

    public string? Violations { get; set; }
}
