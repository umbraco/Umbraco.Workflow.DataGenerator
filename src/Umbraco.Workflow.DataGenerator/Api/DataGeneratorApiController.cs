using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Packaging;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Workflow.Core;
using Umbraco.Workflow.DataGenerator.Models;
using Umbraco.Workflow.DataGenerator.Services;

namespace Umbraco.Workflow.DataGenerator.Api;

[PluginController(Constants.ApplicationName)]
[ApiExplorerSettings(GroupName = "Data generator")]
[Route("/umbraco/backoffice/workflow/generator")]
public sealed class DataGeneratorApiController : UmbracoAuthorizedApiController
{
    private readonly IDataGeneratorService _generatorService;

    public DataGeneratorApiController(IDataGeneratorService generatorService) => _generatorService = generatorService;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] WorkflowDataGeneratorRequestModel model)
    {
        InstallationSummary? result = await _generatorService.TryGenerate(model);
        return Ok(result);
    }

    [HttpPost("reset")]
    public async Task<IActionResult> Reset()
    {
        // status is set to the post-reset values
        GeneratorStatusModel currentStatus = _generatorService.GetStatus();
        GeneratorStatusModel futureStatus = new()
        {
            HasExistingUmbracoData = currentStatus.HasExistingUmbracoData,
            HasInstalledWorkflowData = false,
            HasInstalledUmbracoData = false,
        };

        await _generatorService.Reset(currentStatus, futureStatus);
        return Ok();
    }

    [HttpPost("dismiss-tour")]
    public IActionResult DismissTour()
    {
        _generatorService.DismissTour();
        return Ok();
    }

    [HttpGet("status")]
    public IActionResult Status()
    {
        GeneratorStatusModel status = _generatorService.GetStatus();
        return Ok(status);
    }
}
