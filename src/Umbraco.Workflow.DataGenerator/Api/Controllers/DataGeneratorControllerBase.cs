using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Api.Common.Attributes;
using Umbraco.Cms.Api.Management.Filters;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Workflow.DataGenerator.Api.Configuration;
using Umbraco.Workflow.DataGenerator.Services;

namespace Umbraco.Workflow.DataGenerator.Api.Controllers;

[ApiController]
[MapToApi(ApiConstants.DataGenerationInternalApiName)]
[AppendEventMessages]
public abstract class DataGeneratorControllerBase : ControllerBase
{
    protected IDataGeneratorService GeneratorService { get; }

    private readonly IHttpContextAccessor _httpContextAccessor;
    private DeliveryApiSettings _deliveryApiSettings;

    public DataGeneratorControllerBase(
        IDataGeneratorService generatorService,
        IOptionsMonitor<DeliveryApiSettings> deliveryApiSettings,
        IHttpContextAccessor httpContextAccessor)
    {
        GeneratorService = generatorService;

        _deliveryApiSettings = deliveryApiSettings.CurrentValue;
        _httpContextAccessor = httpContextAccessor;

        deliveryApiSettings.OnChange(settings => _deliveryApiSettings = settings);
    }

    /// <summary>
    /// For now it's okay if we rely on Umbraco's DeliveryApiSettings to determine if the API is public or not.
    /// In the future we may want to add our own configuration for it.
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public bool HasPublicAccess() => _deliveryApiSettings.PublicAccess || HasValidApiKey();

    private bool HasValidApiKey() =>
        string.IsNullOrWhiteSpace(_deliveryApiSettings.ApiKey) is false
            && _deliveryApiSettings.ApiKey.Equals(GetHeaderValue("Api-Key"));

    private string? GetHeaderValue(string headerName) => _httpContextAccessor.HttpContext?.Request?.Headers[headerName];
}
