
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.DTOs;
using YourNamespace.Repositories;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/transactions")]
public class TransactionsController : ControllerBase
{
    private static readonly TransactionService _transactionService;

    static TransactionsController()
    {
        _transactionService = new TransactionService();
    }


    //[HttpPost("start")]
    //public async Task<IActionResult> StartTransaction()
    //{
    //    try
    //    {
    //        await _transactionService.StartTransactionAsync(1);

    //        await _transactionService.StartTransactionAsync(2);
    //        return Ok($"Transaction  started.");
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

    [HttpPost("update")]
    public async Task<IActionResult> UpdateTransaction( int transactionId, AccidentDto accident)
    {
        try
        {
            await _transactionService.UpdateFieldAsync(transactionId, accident);
            return Ok($"Transaction {transactionId} updated with : {accident}");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("commit")]
    public async Task<IActionResult> CommitTransactions()
    {
        try
        {
            await _transactionService.CommitTransactionsAsync();
            return Ok($"Transactions committed.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error committing transaction : {ex.Message}");
        }
    }
}
