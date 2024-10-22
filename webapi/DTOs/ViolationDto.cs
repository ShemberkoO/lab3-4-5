using webapi.Models;

namespace webapi.DTOs;
public partial class ViolationDto
{
    public int ViolationId { get; set; }

    public string Article { get; set; } = null!;

    public string? Comment { get; set; }

    public int? VictimId { get; set; }
}
