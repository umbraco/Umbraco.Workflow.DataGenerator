using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Workflow.DataGenerator.Extensions;

namespace Umbraco.Workflow.DataGenerator.Configuration;

[ComposeAfter(typeof(UmbracoWorkflowComposer))]
public class DataGeneratorComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder) => builder.AddDataGenerator();
}
