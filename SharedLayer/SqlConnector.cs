using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SharedLayer;

public sealed class SqlConnector
{
    private readonly string _conn;
    public SqlConnector(IConfiguration cfg) => _conn = cfg["SqlConnectionString"] ?? throw new("Conn str missing");

    public async Task ExecuteAsync(string sql, IEnumerable<SqlParameter> parameters)
    {
        await using var cn = new SqlConnection(_conn);
        await cn.OpenAsync();
        await using var tx = cn.BeginTransaction();
        try
        {
            await using var cmd = new SqlCommand(sql, cn, tx);
            cmd.Parameters.AddRange(parameters.ToArray());
            await cmd.ExecuteNonQueryAsync();
            await tx.CommitAsync();
        }
        catch { await tx.RollbackAsync(); throw; }
    }
}