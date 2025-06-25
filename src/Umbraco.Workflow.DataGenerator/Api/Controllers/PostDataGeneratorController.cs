using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Workflow.DataGenerator.Api.Configuration;
using Umbraco.Workflow.DataGenerator.Models;
using Umbraco.Workflow.DataGenerator.Services;

namespace Umbraco.Workflow.DataGenerator.Api.Controllers;

[DataGeneratorApiBackofficeRoute("generator")]
[ApiExplorerSettings(GroupName = "Data generator")]
[ApiVersion(1.0)]
public class PostDataGeneratorController : DataGeneratorControllerBase
{
    public PostDataGeneratorController(
        IHttpContextAccessor httpContextAccessor,
        IOptionsMonitor<DeliveryApiSettings> deliveryApiSettings,
        IDataGeneratorService generatorService)
        : base(generatorService, deliveryApiSettings, httpContextAccessor)
    {
    }

    [HttpPost]
    [MapToApiVersion(1.0)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Post([FromBody] WorkflowDataGeneratorRequestModel model)
    {
        if (!HasPublicAccess())
        {
            return Unauthorized();
        }

        bool result = await GeneratorService.TryGenerate(model);
        return Ok(result);
    }
}
