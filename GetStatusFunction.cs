using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureBoilerplateCS;

public class GetStatusFunction
{
    private readonly ILogger<GetStatusFunction> _logger;

    public GetStatusFunction(ILogger<GetStatusFunction> logger)
    {
        _logger = logger;
    }

    [Function("GetStatusFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}
