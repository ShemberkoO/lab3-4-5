using System.Runtime.InteropServices;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using webapi.DTOs;

public class TransactionService
{
    private readonly string _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=45685293;Database=dtp_bd_labs;";
    private readonly Dictionary<int, (NpgsqlConnection connection, NpgsqlTransaction transaction)> _transactions;

    public TransactionService()
    {
        _transactions = new Dictionary<int, (NpgsqlConnection, NpgsqlTransaction)>();
    }

    public async Task StartTransactionAsync(int transactionId)
    {
        if (_transactions.ContainsKey(transactionId))
            RollbackTransactionAsync(transactionId);

        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var transaction = await connection.BeginTransactionAsync(IsolationLevel.Serializable);
        _transactions[transactionId] = (connection, transaction);
    }

    public async Task UpdateFieldAsync(int transactionId, AccidentDto updated)
    {  if(_transactions.Count == 0)
        {
            await StartTransactionAsync(1);
            await StartTransactionAsync(2);

        }

        if (!_transactions.ContainsKey(transactionId))
            throw new InvalidOperationException($"Transaction {transactionId} not started.");

        var (connection, transaction) = _transactions[transactionId];

        // SQL-запит для оновлення даних
        string query = @"
        UPDATE Accidents
        SET 
            date = @Date,
            location = @Location,
            description = @Description
        WHERE 
            accident_id = @AccidentId;";

        using var command = new NpgsqlCommand(query, connection, transaction);

        command.Parameters.AddWithValue("@AccidentId", updated.AccidentId);
        command.Parameters.AddWithValue("@Date", updated.Date.ToDateTime(TimeOnly.MinValue));
        command.Parameters.AddWithValue("@Location", updated.Location);
        command.Parameters.AddWithValue("@Description", (object?)updated.Description ?? DBNull.Value);
        try
        {
            await command.ExecuteNonQueryAsync();
        }catch(Exception e)
        {
            Console.WriteLine("saving exeption: " + e);
            throw e;
        }
      
    }

    public async Task CommitTransactionsAsync()
    {

        foreach(var tr in _transactions)
        {
            await CommitTransactionAsync(tr.Key);
        }
        
    }
    public async Task CommitTransactionAsync(int transactionId)
    {
        if (!_transactions.ContainsKey(transactionId))
            throw new InvalidOperationException($"Transaction {transactionId} not started.");

        var (connection, transaction) = _transactions[transactionId];

        try
        {
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw e; 
        }
        finally
        {
            await connection.CloseAsync();
            _transactions.Remove(transactionId);
        }
    }

    public async Task RollbackTransactionAsync(int transactionId)
    {
        if (!_transactions.ContainsKey(transactionId))
            throw new InvalidOperationException($"Transaction {transactionId} not started.");

        var (connection, transaction) = _transactions[transactionId];
        await transaction.RollbackAsync();
        await connection.CloseAsync();

        _transactions.Remove(transactionId);
    }

    public void DisposeAllTransactions()
    {
        foreach (var (connection, transaction) in _transactions.Values)
        {
            transaction.Dispose();
            connection.Dispose();
        }
        _transactions.Clear();
    }
}
