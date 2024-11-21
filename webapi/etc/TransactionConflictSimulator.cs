using Microsoft.EntityFrameworkCore;
using webapi.Models;
using System.Threading.Tasks;

public class TransactionConflictSimulator
{
    private readonly DtpBdLabsContext _context;

    public TransactionConflictSimulator()
    {
        
    }

    public async Task SimulateConflict()
    {  // Створюємо два контексти для паралельних транзакцій
        var _context1 = new DtpBdLabsContext();
        var _context2 = new DtpBdLabsContext();
        
            Console.WriteLine("Started Transactions");




            var accident1 = await _context1.Accidents.Where(a => a.AccidentId == 11).FirstOrDefaultAsync();
            var accident2 = await _context2.Accidents.Where(a => a.AccidentId == 11).FirstOrDefaultAsync();
            Console.WriteLine($"Before Accident Description: {accident1?.Description} ||  {accident2?.Description}");

      
        var task1 = Task.Run(async () =>
        {
            const int maxRetries = 3; // Максимальна кількість спроб
            int attempt = 0;
            bool success = false;

            using var transaction1 = await _context1.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

            while (attempt < maxRetries && !success)
            {
                attempt++;
                try
                {
                    var accident1 = await _context1.Accidents
                        .Where(a => a.AccidentId == 11)
                        .FirstOrDefaultAsync();

                    if (accident1 != null)
                    {
                        accident1.Description = accident1.Description + "1"; // Оновлюємо запис
                        await _context1.SaveChangesAsync();
                        Console.WriteLine("Transaction 1: Updated accident.");
                    }

                    await transaction1.CommitAsync();  // Коміт транзакції
                    success = true; // Позначаємо, що транзакція виконана успішно
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error in attempt {attempt} tr 1: {e}");

                    if (attempt < maxRetries)
                    {
                        Console.WriteLine("Retrying transaction after 1 second...");
                        await Task.Delay(1000); // Затримка перед повтором
                    }
                    else
                    {
                        await transaction1.RollbackAsync(); // Відкат транзакції
                        Console.WriteLine("Max retry attempts reached. Transaction 1 failed.");
                    }
                }
            }
        });


        var task2 = Task.Run(async () =>
        {
            const int maxRetries = 3; // Максимальна кількість спроб
            int attempt = 0;
            bool success = false;

            while (attempt < maxRetries && !success)
            {
                attempt++;
                try
                {
                    using var context2 = new DtpBdLabsContext(); // Новий контекст
                    using var transaction2 = await context2.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

                    var accident2 = await context2.Accidents
                        .Where(a => a.AccidentId == 11)
                        .FirstOrDefaultAsync();

                    if (accident2 != null)
                    {
                        accident2.Description = "Updated by 2"; // Оновлюємо запис
                        await context2.SaveChangesAsync();
                        Console.WriteLine("Transaction 2: Updated accident.");
                    }

                    await transaction2.CommitAsync(); // Коміт транзакції
                    success = true; // Позначаємо, що транзакція виконана успішно
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error in attempt {attempt} tr 2: {e}");

                    if (attempt < maxRetries)
                    {
                        Console.WriteLine("Retrying transaction 2 after 1 second...");
                        await Task.Delay(1000); // Затримка перед повторною спробою
                    }
                    else
                    {
                        Console.WriteLine("Max retry attempts reached. Transaction 2 failed.");
                    }
                }
            }
        });




        await Task.WhenAll(task1, task2);

            // Підсумковий вивід
            var fa = await _context1.Accidents.Where(a => a.AccidentId == 11).FirstOrDefaultAsync();
            var finalAccident = await _context2.Accidents.Where(a => a.AccidentId == 11).FirstOrDefaultAsync();
            Console.WriteLine($"Final Accident Description: {finalAccident?.Description} ||  {fa?.Description}");

     
    }
}
