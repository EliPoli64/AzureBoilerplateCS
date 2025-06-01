using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;
using Data;
using Data.Entities;
using SharedLayer;
using Microsoft.EntityFrameworkCore;

namespace OrmFunctions;

public class Votar(ContextoVotoDB db)
{
    [Function("Votar")] 
    public async Task<HttpResponseData> Run([
        HttpTrigger(AuthorizationLevel.Function, "post", Route = "votar")] HttpRequestData req)
    {
        var dto = await JsonSerializer.DeserializeAsync<VotoDto>(req.Body);
        if (dto is null) return req.CreateResponse(HttpStatusCode.BadRequest);

        if (await db.Votos.AnyAsync(v => v.UsuarioId == dto.UsuarioId && v.PropuestaId == dto.PropuestaId))
            return req.CreateResponse(HttpStatusCode.Conflict);

        db.Votos.Add(new Voto
        {
            PropuestaId = dto.PropuestaId,
            UsuarioId = dto.UsuarioId,
            Fecha = DateTime.UtcNow,
            VotoCifrado = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(dto.VotoPlano))
        });
        await db.SaveChangesAsync();

        var res = req.CreateResponse(HttpStatusCode.OK);
        await res.WriteStringAsync("Voto registrado");
        return res;
    }
}