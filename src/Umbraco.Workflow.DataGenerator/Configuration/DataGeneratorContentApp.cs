using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Workflow.Core;

namespace Umbraco.Workflow.DataGenerator.Configuration;

/// <summary>
///
/// </summary>
internal sealed class DataGeneratorContentApp : IContentAppFactory
{
    public ContentApp? GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups)
    {
        // content app shows on content only, where type is not an element
        if (source is IContent content && !content.ContentType.IsElement)
        {
            return new ContentApp
            {
                Alias = Constants.ApplicationName.ToLower(),
                Name = Constants.ApplicationName,
                Icon = Constants.ApplicationIcon,
                View = "/App_Plugins/Umbraco.Workflow.DataGenerator/dashboard.html",
                Weight = 0,
            };
        }

        return null;
    }
}
