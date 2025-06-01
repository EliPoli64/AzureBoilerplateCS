using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using SharedLayer;
using Microsoft.Extensions.Configuration;

namespace ProceduresFunctions;

public class RevisarPropuesta(IConfiguration cfg)
{
  [Function("RevisarPropuesta")]
  public async Task<HttpResponseData> Run([
      HttpTrigger(AuthorizationLevel.Function, "post", Route = "propuesta/{id:int}/revisar")] HttpRequestData req, int id)
  {
    var sql = "EXEC dbo.sp_RevisarPropuesta @Id";
    await new SqlConnector(cfg).ExecuteAsync(sql, [new("@Id", id)]);
    var res = req.CreateResponse(HttpStatusCode.OK);
    await res.WriteStringAsync("Revisión enviada");
    return res;
  }
}