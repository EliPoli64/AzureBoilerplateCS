using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;

namespace PostInsertDataFunction
{
    public class PostInsertDataFunction
    {
        [Function("PostInsertData")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "records")] HttpRequestData req)
        {
            InfoIA? requestBody;
            try
            {
                requestBody = await JsonSerializer.DeserializeAsync<InfoIA>(req.Body);
            }
            catch (JsonException ex)
            {
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                await bad.WriteStringAsync($"Invalid JSON format: {ex.Message}");
                return bad;
            }

            if (requestBody is null || requestBody.infoId == 0 || string.IsNullOrWhiteSpace(requestBody.modeloIA))
            {
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                await bad.WriteStringAsync("Invalid input data. 'infoId' must be non‑zero and 'modeloIA' cannot be empty.");
                return bad;
            }

            string? connString = Environment.GetEnvironmentVariable("SqlConnectionString");
            if (string.IsNullOrEmpty(connString))
            {
                var err = req.CreateResponse(HttpStatusCode.InternalServerError);
                await err.WriteStringAsync("Connection string not configured.");
                return err;
            }

            const string insertSql = @"INSERT INTO dbo.pv_infoIA (infoId, modeloIA, apiKey, token, maxTokens)
                                       VALUES (@infoId, @modeloIA, @apiKey, @token, @maxTokens);";

            try
            {
                await using var connection = new SqlConnection(connString);
                await connection.OpenAsync();
                await using var command = new SqlCommand(insertSql, connection);
                command.Parameters.AddWithValue("@infoId", requestBody.infoId);
                command.Parameters.AddWithValue("@modeloIA", requestBody.modeloIA);
                command.Parameters.AddWithValue("@apiKey", (object?)requestBody.apiKey ?? DBNull.Value);
                command.Parameters.AddWithValue("@token", (object?)requestBody.token ?? DBNull.Value);
                command.Parameters.AddWithValue("@maxTokens", requestBody.maxTokens);

                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                var err = req.CreateResponse(HttpStatusCode.InternalServerError);
                await err.WriteStringAsync($"Database error: {ex.Message}");
                return err;
            }
            catch (Exception ex)
            {
                var err = req.CreateResponse(HttpStatusCode.InternalServerError);
                await err.WriteStringAsync($"Unexpected error: {ex.Message}");
                return err;
            }

            var response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteStringAsync("Registro creado en pv_infoIA correctamente.");
            return response;
        }
    }

    public class InfoIA
    {
        public int infoId { get; set; }
        public string modeloIA { get; set; } = string.Empty;
        public string? apiKey { get; set; }
        public string? token { get; set; }
        public int maxTokens { get; set; }
    }
}