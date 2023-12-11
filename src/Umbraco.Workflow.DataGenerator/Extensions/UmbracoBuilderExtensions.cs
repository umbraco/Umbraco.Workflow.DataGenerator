using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Workflow.DataGenerator.Configuration;
using Umbraco.Workflow.DataGenerator.Configuration.Umbraco;
using Umbraco.Workflow.DataGenerator.Configuration.Workflow;
using Umbraco.Workflow.DataGenerator.NotificationHandlers;
using Umbraco.Workflow.DataGenerator.Services;

namespace Umbraco.Workflow.DataGenerator.Extensions;

public static class UmbracoBuilderExtensions
{
    internal static IUmbracoBuilder AddDataGenerator(this IUmbracoBuilder builder)
    {
        _ = builder.Services.Configure<DataGeneratorSettings>(builder.Config.GetSection(DataGeneratorSettings.OptionsName));

        _ = builder.Services
            .AddTransient<IDataGeneratorService, DataGeneratorService>()
            .AddTransient<IMediaImporter, MediaImporter>()
            .AddTransient<IUmbracoHousekeeper, UmbracoHousekeeper>()
            .AddTransient<IWorkflowConfigurator, WorkflowConfigurator>()
            .AddTransient<IWorkflowDestructor, WorkflowDestructor>()
            .AddTransient<IPackageInstaller, PackageInstaller>();

        _ = builder.AddNotificationHandler<ServerVariablesParsingNotification, ServerVariablesParsingNotificationHandler>();

        return builder;
    }
}
