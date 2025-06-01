using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SharedLayer
{
    public sealed class SqlConnector
    {
        private readonly string _connectionString;

        public SqlConnector(IConfiguration configuration)
        {
            _connectionString = configuration["SqlConnectionString"]
                ?? throw new ArgumentNullException(nameof(configuration), "Environment variable 'SqlConnectionString' not found.");
        }

        public async Task<int> ExecuteAsync(string sql, IEnumerable<SqlParameter> parameters)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddRange(parameters.ToArray());
            return await command.ExecuteNonQueryAsync();
        }

        public async Task<List<T>> QueryAsync<T>(string sql, Func<SqlDataReader, T> map, IEnumerable<SqlParameter> parameters)
        {
            var results = new List<T>();
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddRange(parameters.ToArray());
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(map(reader));
            }
            return results;
        }
    }
}