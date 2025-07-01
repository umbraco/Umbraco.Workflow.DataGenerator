using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Workflow.DataGenerator.Api.Configuration;
using Umbraco.Workflow.DataGenerator.Configuration;
using Umbraco.Workflow.DataGenerator.Services;

namespace Umbraco.Workflow.DataGenerator.Extensions;

public static class UmbracoBuilderExtensions
{
    public static IUmbracoBuilder AddDataGenerator(this IUmbracoBuilder builder)
    {
        _ = builder.Services
            .AddTransient<IDataGeneratorService, DataGeneratorService>()
            .AddTransient<IWorkflowConfigurator, WorkflowConfigurator>()
            .AddTransient<IWorkflowDestructor, WorkflowDestructor>();

        _ = builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(ApiConstants.DataGenerationInternalApiName, new OpenApiInfo
            {
                Title = "Umbraco Workflow Data Generator API",
                Version = "Latest",
                Description = string.Empty,
            });

            c.OperationFilter<SwaggerDocumentationFilter>();
            c.ParameterFilter<SwaggerDocumentationFilter>();
        });

        return builder;
    }
}
