namespace OrmFunctions;
using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using SharedLayer;
using Microsoft.Extensions.Configuration;
using Data;
public class ConfigurarVotacion(ContextoVotoDB db)
{
    [Function("ConfigurarVotacion")]
    public async Task<HttpResponseData> Run([
        HttpTrigger(AuthorizationLevel.Function, "post", Route = "propuesta/{id:int}/configurar")] HttpRequestData req, int id)
    {
        // TODO: Implementar reglas complejas según DTO
        var prop = await db.Propuestas.FindAsync(id);
        if (prop is null) return req.CreateResponse(HttpStatusCode.NotFound);
        prop.Estado = "configurada";
        await db.SaveChangesAsync();
        var res = req.CreateResponse(HttpStatusCode.OK);
        await res.WriteStringAsync("Votación configurada");
        return res;
    }
}