using Newtonsoft.Json;

namespace Umbraco.Workflow.DataGenerator.Models;

public sealed class GeneratorStatusModel
{
    [JsonProperty("hasExistingUmbracoData")]
    public bool HasExistingUmbracoData { get; set; }

    [JsonProperty("hasInstalledUmbracoData")]
    public bool HasInstalledUmbracoData { get; set; }

    [JsonProperty("hasInstalledWorkflowData")]
    public bool HasInstalledWorkflowData { get; set; }

    [JsonProperty("hasDismissedTour")]
    public bool HasDismissedTour { get; set; }

    [JsonProperty("workflowDataModel")]
    public WorkflowDataGeneratorRequestModel? WorkflowDataModel { get; set; }
}
