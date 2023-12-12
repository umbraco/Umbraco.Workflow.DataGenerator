using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Workflow.DataGenerator.Configuration;
using Umbraco.Workflow.DataGenerator.NotificationHandlers;
using Umbraco.Workflow.DataGenerator.Services;

namespace Umbraco.Workflow.DataGenerator.Extensions;

public static class UmbracoBuilderExtensions
{
    internal static IUmbracoBuilder AddDataGenerator(this IUmbracoBuilder builder)
    {
        _ = builder.Services
            .AddTransient<IDataGeneratorService, DataGeneratorService>()
            .AddTransient<IWorkflowConfigurator, WorkflowConfigurator>()
            .AddTransient<IWorkflowDestructor, WorkflowDestructor>();

        _ = builder.AddNotificationHandler<ServerVariablesParsingNotification, ServerVariablesParsingNotificationHandler>();

        return builder;
    }
}
