namespace OrmFunctions;
using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Data;
using Microsoft.EntityFrameworkCore;

public class ListarVotos(ContextoVotoDB db)
{
    [Function("ListarVotos")]
    public async Task<HttpResponseData> Run([
        HttpTrigger(AuthorizationLevel.Function, "get", Route = "votos/{usuarioId:int}")] HttpRequestData req, int usuarioId)
    {
        var votos = await db.Votos
            .Where(v => v.UsuarioId == usuarioId)
            .OrderByDescending(v => v.Fecha)
            .Take(5)
            .Select(v => new { v.PropuestaId, v.Fecha })
            .ToListAsync();

        var res = req.CreateResponse(HttpStatusCode.OK);
        await res.WriteStringAsync(JsonSerializer.Serialize(votos));
        return res;
    }
}