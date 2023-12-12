using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Workflow.DataGenerator.Models;
using Umbraco.Workflow.DataGenerator.Services;

namespace Umbraco.Workflow.DataGenerator.Api;

[PluginController("Workflow")]
[ApiExplorerSettings(GroupName = "Data generator")]
[Route("/umbraco/backoffice/workflow/generator")]
public sealed class DataGeneratorApiController : UmbracoAuthorizedApiController
{
    private readonly IDataGeneratorService _generatorService;

    public DataGeneratorApiController(IDataGeneratorService generatorService) => _generatorService = generatorService;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] WorkflowDataGeneratorRequestModel model)
    {
        bool result = await _generatorService.TryGenerate(model);
        return Ok(result);
    }

    [HttpPost("reset")]
    public async Task<IActionResult> Reset()
    {
        await _generatorService.Reset();
        return Ok();
    }

    [HttpGet("status")]
    public IActionResult Status()
    {
        bool status = _generatorService.GetStatus();
        return Ok(status);
    }
}
