using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.DTOs; // Import the DTOs namespace
using webapi.Models;

[Route("api/[controller]")]
[ApiController]
public class VictimsController : ControllerBase
{
    private readonly DtpBdLabsContext _context;

    public VictimsController(DtpBdLabsContext context)
    {
        _context = context;
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<VictimDto>> GetVictim(int Id)
    {
        var victim = await _context.Victims.FindAsync(Id);

        if (victim == null)
        {
            return NotFound();
        }
        var victimDto = new VictimDto()
        {
            PasportId = victim.PasportId,
            AccidentId = victim.AccidentId,
            VictimId = victim.VictimId,
            Status = victim.Status
        };
  
        return Ok(victimDto);
    }
}
