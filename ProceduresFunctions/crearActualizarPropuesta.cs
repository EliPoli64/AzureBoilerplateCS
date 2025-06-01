using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using SharedLayer;
using Microsoft.Extensions.Configuration;

namespace ProceduresFunctions;

public class CrearActualizarPropuesta(IConfiguration cfg)
{
    [Function("CrearActualizarPropuesta")]
    public async Task<HttpResponseData> Run([
        HttpTrigger(AuthorizationLevel.Function, "post", Route = "propuesta")] HttpRequestData req)
    {
        var dto = await JsonSerializer.DeserializeAsync<PropuestaDto>(req.Body);
        if (dto is null)
            return req.CreateResponse(HttpStatusCode.BadRequest);

        var sp = "EXEC dbo.sp_CrearActualizarPropuesta @Id,@Titulo,@Descripcion,@UsuarioId";
        var parameters = new[]
        {
            new SqlParameter("@Id", dto.Id),
            new SqlParameter("@Titulo", dto.Titulo),
            new SqlParameter("@Descripcion", dto.Descripcion),
            new SqlParameter("@UsuarioId", dto.UsuarioId)
        };

        await new SqlConnector(cfg).ExecuteAsync(sp, parameters);
        var res = req.CreateResponse(HttpStatusCode.OK);
        await res.WriteStringAsync("Propuesta procesada");
        return res;
    }
}