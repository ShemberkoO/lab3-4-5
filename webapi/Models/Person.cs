using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class Person
{
    public string PasportId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string RegistrationAddress { get; set; } = null!;

    public virtual ICollection<Transport> Transports { get; set; } = new List<Transport>();

    public virtual ICollection<Victim> Victims { get; set; } = new List<Victim>();
}
