using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class Transport
{
    public int TransportId { get; set; }

    public string Type { get; set; } = null!;

    public string? RegistrationNumber { get; set; }

    public string? Brand { get; set; }

    public string? Model { get; set; }

    public int? YearOfManufacture { get; set; }

    public string? OwnerId { get; set; }

    public virtual ICollection<OccupantTransport> OccupantTransports { get; set; } = new List<OccupantTransport>();

    public virtual Person? Owner { get; set; }
}
