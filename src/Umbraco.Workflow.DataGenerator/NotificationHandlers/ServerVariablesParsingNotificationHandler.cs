using Microsoft.AspNetCore.Routing;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;
using Umbraco.Workflow.DataGenerator.Api;

namespace Umbraco.Workflow.DataGenerator.NotificationHandlers;

internal sealed class ServerVariablesParsingNotificationHandler : INotificationHandler<ServerVariablesParsingNotification>
{
    private readonly IRuntimeState _runtimeState;
    private readonly LinkGenerator _linkGenerator;

    public ServerVariablesParsingNotificationHandler(LinkGenerator linkGenerator, IRuntimeState runtimeState)
    {
        _linkGenerator = linkGenerator;
        _runtimeState = runtimeState;
    }

    public void Handle(ServerVariablesParsingNotification notification)
    {
        if (_runtimeState.Level != RuntimeLevel.Run)
        {
            return;
        }

        Dictionary<string, object?> serverVars = new()
        {
            ["apiBaseUrl"] = _linkGenerator.GetUmbracoApiServiceBaseUrl<DataGeneratorApiController>(x => x.Reset()),
        };

        if (TryGetServerVars("umbracoSettings") is IDictionary<string, object?> umbracoSettings)
        {
            serverVars["pluginPath"] = $"{umbracoSettings["appPluginsPath"]}/Workflow.DataGenerator/backoffice";
        }

        notification.ServerVariables.Add("UmbracoWorkflowDataGenerator", serverVars);

        IDictionary<string, object?>? TryGetServerVars(string key)
        {
            if (notification.ServerVariables.TryGetValue(key, out var serverVarsObject) &&
                serverVarsObject is IDictionary<string, object?> serverVarsDictionary)
            {
                return serverVarsDictionary;
            }

            return null;
        }
    }
}
