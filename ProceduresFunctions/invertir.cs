using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using SharedLayer;
using Microsoft.Extensions.Configuration;

namespace ProceduresFunctions;

public class Invertir(IConfiguration cfg)
{
  [Function("Invertir")]
  public async Task<HttpResponseData> Run([
      HttpTrigger(AuthorizationLevel.Function, "post", Route = "invertir")] HttpRequestData req)
  {
    var dto = await JsonSerializer.DeserializeAsync<InversionDto>(req.Body);
    if (dto is null) return req.CreateResponse(HttpStatusCode.BadRequest);

    var sql = "EXEC dbo.sp_Invertir @PropuestaId,@UsuarioId,@Monto";
    var p = new[]
    {
            new SqlParameter("@PropuestaId", dto.PropuestaId),
            new SqlParameter("@UsuarioId", dto.UsuarioId),
            new SqlParameter("@Monto", dto.Monto)
        };
    await new SqlConnector(cfg).ExecuteAsync(sql, p);
    var res = req.CreateResponse(HttpStatusCode.OK);
    await res.WriteStringAsync("Inversión registrada");
    return res;
  }
}