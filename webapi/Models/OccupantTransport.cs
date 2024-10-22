using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class OccupantTransport
{
    public int VictimId { get; set; }

    public int TransportId { get; set; }

    public string? DriverLicenseNumber { get; set; }

    public virtual Transport Transport { get; set; } = null!;

    public virtual Victim Victim { get; set; } = null!;
}
