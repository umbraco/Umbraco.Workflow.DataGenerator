using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Workflow.DataGenerator.Api.Configuration;
using Umbraco.Workflow.DataGenerator.Services;

namespace Umbraco.Workflow.DataGenerator.Api.Controllers;

[DataGeneratorApiBackofficeRoute("generator")]
[ApiExplorerSettings(GroupName = "Data generator")]
[ApiVersion(1.0)]
public class ResetDataGeneratorController : DataGeneratorControllerBase
{
    public ResetDataGeneratorController(
        IHttpContextAccessor httpContextAccessor,
        IOptionsMonitor<DeliveryApiSettings> deliveryApiSettings,
        IDataGeneratorService generatorService)
        : base(generatorService, deliveryApiSettings, httpContextAccessor)
    {
    }

    [HttpPost("reset")]
    [MapToApiVersion(1.0)]
    [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Reset()
    {
        if (!HasPublicAccess())
        {
            return Unauthorized();
        }

        await GeneratorService.Reset();
        return Ok();
    }
}
