using Npgsql;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class PostgresTransactionConflictSimulator
{
    private readonly string _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=45685293;Database=dtp_bd_labs;";

    public async Task SimulateConflict()
    {
        // Створюємо два з'єднання для паралельних транзакцій
        var connection1 = new NpgsqlConnection(_connectionString);
         var connection2 = new NpgsqlConnection(_connectionString);

        await connection1.OpenAsync();
        await connection2.OpenAsync();

        try
        {
            var finalCommand1 = new NpgsqlCommand("SELECT description FROM accidents WHERE accident_id = 11", connection1);
            var finalDescription1 = (string)await finalCommand1.ExecuteScalarAsync();
            Console.WriteLine($"Before  Description (via connection 1): {finalDescription1}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }


        try
        {
            var finalCommand2 = new NpgsqlCommand("SELECT description FROM accidents WHERE accident_id = 11", connection2);
            var finalDescription2 = (string)await finalCommand2.ExecuteScalarAsync();
            Console.WriteLine($"Before  Description (via connection 2): {finalDescription2}");
        }
        catch (Exception e)
        {

            Console.WriteLine(e);
        }


        // Починаємо транзакції для обох з'єднань
         var transaction1 = await connection1.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
         var transaction2 = await connection2.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

        // Паралельно запускаємо два завдання для оновлення одних і тих самих даних
        var task1 = Task.Run(async () =>
        {
            Thread.Sleep(100);
            try
            {
                // Отримуємо дані для першої транзакції
                var command1 = new NpgsqlCommand("SELECT description FROM accidents WHERE accident_id = 11", connection1, transaction1);
                var description1 = (string)await command1.ExecuteScalarAsync();
                Console.WriteLine($"Before Transaction 1 Description: {description1}");

                // Оновлюємо запис
                var updateCommand1 = new NpgsqlCommand("UPDATE accidents SET description = description || '1' WHERE accident_id = 11", connection1, transaction1);
                await updateCommand1.ExecuteNonQueryAsync();
                Console.WriteLine("Transaction 1: Updated accident.");

                // Зберігаємо зміни і комітимо
               
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in Transaction 1: " + e.Message);
            }finally
            {
                try {
                    await transaction1.CommitAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
               
            }
        });

        var task2 = Task.Run(async () =>
        {
            Thread.Sleep(100);
            try
            {
                // Отримуємо дані для другої транзакції
                var command2 = new NpgsqlCommand("SELECT description FROM accidents WHERE accident_id = 11", connection2, transaction2);
                var description2 = (string)await command2.ExecuteScalarAsync();
                Console.WriteLine($"Before Transaction 2 Description: {description2}");

                // Оновлюємо запис
                var updateCommand2 = new NpgsqlCommand("UPDATE accidents SET description = 'Updated by 2' WHERE accident_id = 11", connection2, transaction2);
                await updateCommand2.ExecuteNonQueryAsync();
                Console.WriteLine("Transaction 2: Updated accident.");

                // Зберігаємо зміни і комітимо
                await transaction2.CommitAsync();
            }
            catch (Exception e)
            {
               
                Console.WriteLine("Error in Transaction 2: " + e.Message);
            }
            finally
            {
                try
                {
                    await transaction2.CommitAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        });

        // Чекаємо, поки обидва завдання завершаться
        await Task.WhenAll(task1 , task2);

        // Підсумковий вивід
        

        try
        {
            var finalCommand1 = new NpgsqlCommand("SELECT description FROM accidents WHERE accident_id = 11", connection1);
            var finalDescription1 = (string)await finalCommand1.ExecuteScalarAsync();
            Console.WriteLine($"Final Accident Description (via connection 1): {finalDescription1}");
        }
        catch(Exception e)
        {

            Console.WriteLine("Can not get connection1 info ");
            Console.WriteLine(e);
        }


        try
        {
            var finalCommand2 = new NpgsqlCommand("SELECT description FROM accidents WHERE accident_id = 11", connection2);
            var finalDescription2 = (string)await finalCommand2.ExecuteScalarAsync();
            Console.WriteLine($"Final Accident Description (via connection 2): {finalDescription2}");
        }
        catch (Exception e)
        {


            Console.WriteLine("Can not get connection2 info ");
            Console.WriteLine(e);
        }
      
    }
}
