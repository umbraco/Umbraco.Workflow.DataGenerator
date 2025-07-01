using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Umbraco.Extensions;

namespace Umbraco.Workflow.DataGenerator.Api.Configuration;

public class SwaggerDocumentationFilter : IOperationFilter, IParameterFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.HasMapToApiAttribute(ApiConstants.DataGenerationInternalApiName))
        {
            ApplyTagBasedParameters(operation, context);
        }
    }

    private void ApplyTagBasedParameters(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= [];
        AddApiKeyParameter(operation);
    }

    /// <summary>
    /// Based on Umbraco's implementation on SwaggerDocumentationFilterBase.cs
    /// </summary>
    /// <param name="operation"></param>
    private void AddApiKeyParameter(OpenApiOperation operation) =>
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Api-Key",
            In = ParameterLocation.Header,
            Required = false,
            Description = "API key specified through configuration to authorize access to the API.",
            Schema = new OpenApiSchema { Type = "string" },
        });

    public void Apply(OpenApiParameter parameter, ParameterFilterContext context) { }
}
