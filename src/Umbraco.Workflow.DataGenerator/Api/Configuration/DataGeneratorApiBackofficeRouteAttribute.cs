using Umbraco.Cms.Web.Common.Routing;

namespace Umbraco.Workflow.DataGenerator.Api.Configuration;

internal sealed class DataGeneratorApiBackofficeRouteAttribute : BackOfficeRouteAttribute
{
    public DataGeneratorApiBackofficeRouteAttribute(string template)
        : base($"{ApiConstants.RootPath}/v{{version:apiVersion}}/{template.TrimStart('/')}")
    {
    }
}
