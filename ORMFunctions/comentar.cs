namespace OrmFunctions;
using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using SharedLayer;
using Data;

public class Comentar(ContextoVotoDB db)
{
    [Function("Comentar")]
    public async Task<HttpResponseData> Run([
        HttpTrigger(AuthorizationLevel.Function, "post", Route = "comentar")] HttpRequestData req)
    {
        var dto = await JsonSerializer.DeserializeAsync<ComentarioDto>(req.Body);
        if (dto is null) return req.CreateResponse(HttpStatusCode.BadRequest);

        db.Comentarios.Add(new Data.Entities.Comentario
        {
            PropuestaId = dto.PropuestaId,
            UsuarioId = dto.UsuarioId,
            Contenido = dto.Contenido,
            Estado = "pendiente"
        });
        await db.SaveChangesAsync();
        var res = req.CreateResponse(HttpStatusCode.OK);
        await res.WriteStringAsync("Comentario enviado");
        return res;
    }
}