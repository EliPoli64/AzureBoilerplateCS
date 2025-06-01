using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using SharedLayer;
using Microsoft.Extensions.Configuration;

namespace ProceduresFunctions;

public class RepartirDividendos(IConfiguration cfg)
{
  [Function("RepartirDividendos")]
  public async Task<HttpResponseData> Run([
      HttpTrigger(AuthorizationLevel.Function, "post", Route = "dividendos")] HttpRequestData req)
  {
    var dto = await JsonSerializer.DeserializeAsync<InversionDto>(req.Body);
    if (dto is null) return req.CreateResponse(HttpStatusCode.BadRequest);
    await new SqlConnector(cfg).ExecuteAsync("EXEC dbo.sp_RepartirDividendos", []);
    var r = req.CreateResponse(HttpStatusCode.OK);
    await r.WriteStringAsync("Dividendos procesados");
    return r;
  }
}