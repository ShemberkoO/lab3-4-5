using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using webapi.DTOs;
using webapi.Models;
using YourNamespace.Repositories;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccidentsController : ControllerBase
    {

            
        private static readonly DtpBdLabsContext _context = new DtpBdLabsContext();
        private static  IDbContextTransaction current_transaction;


        private readonly IAccidentRepository _accidentRepository;

        public AccidentsController(IAccidentRepository accidentRepository)
        {
            _accidentRepository = accidentRepository;
            _accidentRepository.setContext(_context);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccidentDto>>> GetAccidents(
         [FromQuery] int? year,
         [FromQuery] string? location,
         [FromQuery] string? query)
        {
            var accidents = await _accidentRepository.GetAccidentsAsync();

            // Фільтр за роком
            if (year.HasValue)
            {
                accidents = accidents.Where(a => a.Date.Year == year.Value);
            }

           
            if (!string.IsNullOrEmpty(location))
            {
                accidents = accidents.Where(a => a.Location.Contains(location, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(query))
            {
                string lowerQuery = query.ToLower();
                accidents = accidents.Where(a =>
                    a.Location.ToLower().Contains(lowerQuery) ||
                    (a.Description != null && a.Description.ToLower().Contains(lowerQuery))
                );
            }

            return Ok(accidents);
        }


        [HttpGet("{id}/show")]
        public async Task<ActionResult<AccidentDto>> GetAccident(int id)
        {
            var accident = await _accidentRepository.GetAccidentByIdAsync(id);
            if (accident == null)
            {
                return NotFound();
            }
            return Ok(accident);
        }

        // POST: api/Accidents
        [HttpPost]
        public async Task<ActionResult<AccidentDto>> PostAccident(AccidentDto accidentDto)
        {
            try
            {
                var accident = await _accidentRepository.CreateAccidentAsync(accidentDto);
                accidentDto.AccidentId = accident.AccidentId;
                return CreatedAtAction(nameof(GetAccident), new { id = accident.AccidentId }, accidentDto);
            }
            catch (DbUpdateException e)
            {
                return StatusCode(401, new { message = "Error: " + e.Message, details = e.InnerException?.Message });
            }


        }

        // PUT: api/Accidents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccident(int id, AccidentDto accidentDto)
        {
            try
            {

                var result = await UpdateAccidentAsync_(id, accidentDto);

                return Ok(result);
            }
            catch (DbUpdateException e)
            {
                return StatusCode(401, new { message = "Error: " + e.Message, details = e.InnerException?.Message });
            }

        }

        [HttpPost("StartTrans")]
        public async Task<IActionResult> StartTrans()
        {
           return Ok( await StartTransactionAsync_());
        }


        [HttpPost("commit")]
        public async Task<IActionResult> Commit()
        {
            return Ok( await Commit_());
        }


        [HttpPost("DeleteAccidentsBeforeDate")]
        public async Task<IActionResult> DeleteAccidentsBeforeDate(DeleteAccidentsInfo cutoffDate)
        {
            if (cutoffDate.Date == default)
            {
                return BadRequest(new { message = "Invalid date provided." });
            }

            try
            {
                var resultMessage = await _accidentRepository.DeleteAccidentsBeforeDateAsync(cutoffDate);
                return Ok(new { message = resultMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error deleting accidents: {ex.Message}" });
            }
        }


        [HttpPost("SimulateConflict")]
        public async Task SimulateConflict()
        {

            Console.WriteLine("Started Method");

            new TransactionConflictSimulator().SimulateConflict();
       
            return;
        }



        // DELETE: api/Accidents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccident(int id)
        {
            var result = await _accidentRepository.DeleteAccidentAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }


        private async Task<string> UpdateAccidentAsync_(int id, AccidentDto accidentDto)
        {
            if (current_transaction != null)
            {
                
            }
            if (id != accidentDto?.AccidentId)
            {
                return "Params not valid";
            }

            var accident = await _context.Accidents.FindAsync(id);
            if (accident == null)
            {
                return "Accident not found";
            }

            accident.Date = accidentDto.Date;
            accident.Location = accidentDto.Location;
            accident.Description = accidentDto.Description;

            try
            {
                await _context.SaveChangesAsync();
                return "Changes saved.";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return "Concurrency error while saving: " + ex.Message;
            }
            catch (DbUpdateException ex)
            {
                return "Database update error: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Unexpected error: " + ex.Message;
            }
        }

        private async Task<string> StartTransactionAsync_()
        {
            string msg = "";
            if (current_transaction != null)
            {
                try
                {
                    await current_transaction.CommitAsync();
                    msg += "\nprevious transaction commited\n";
                }
                catch (InvalidOperationException ex)
                {
                    current_transaction.Dispose();
                    msg += "\nprevious transaction disposed\n";
                   // return "Transaction already completed or disposed: " + ex.Message;
                }
                catch (Exception ex)
                {
                 
                    Console.WriteLine(ex.Message);
                }
            }

            try
            {
                current_transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
                return  msg +"\nTransaction started.\n";
            }
            catch (InvalidOperationException ex)
            {
                return "Failed to start transaction: " + ex.Message;
            }
            catch (DbUpdateException ex)
            {
                return "Database update error: " + ex.Message;
            }
        }


        private async Task<string> Commit_()
        {
            if (current_transaction != null)
            {
                try
                {
                    await current_transaction.CommitAsync();
                    return "Committed successfully.";
                }
                catch (InvalidOperationException ex)
                {
                    return "Invalid operation during commit: " + ex.Message;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    await current_transaction.RollbackAsync();
                    current_transaction.Dispose();
                    return "Concurrency error during commit: " + ex.Message;
                }
                catch (Exception e)
                {
                    try
                    {
                        await current_transaction.RollbackAsync();
                        current_transaction.Dispose();
                    }
                    catch (Exception rollbackEx)
                    {
                        return "Rollback failed: " + rollbackEx.Message;
                    }

                    return "Unexpected error during commit: " + e.Message;
                }
            }
            else
            {
                return "No active transaction.";
            }
        }

    }
}
