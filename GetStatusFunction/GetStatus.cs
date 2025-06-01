using System;
using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace GetStatusFunction
{
    public sealed class GetStatus
    {
        [Function("GetStatus")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "status")] HttpRequestData request)
        {
            var response = request.CreateResponse(HttpStatusCode.OK);
            var payload = new { status = "🟢 Running", timestamp = DateTime.UtcNow };
            response.Headers.Add("Content-Type", "application/json");
            response.WriteString(JsonSerializer.Serialize(payload));
            return response;
        }
    }
}
