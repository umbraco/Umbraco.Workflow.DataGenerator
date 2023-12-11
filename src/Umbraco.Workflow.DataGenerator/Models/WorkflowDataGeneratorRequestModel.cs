using Newtonsoft.Json;

namespace Umbraco.Workflow.DataGenerator.Models;

public sealed class WorkflowDataGeneratorRequestModel
{
    [JsonProperty("userCount")]
    public int UserCount { get; set; }

    [JsonProperty("groupCount")]
    public int GroupCount { get; set; }

    [JsonProperty("usersPerGroup")]
    public int UsersPerGroup { get; set; }

    [JsonProperty("groupsPerWorkflow")]
    public int GroupsPerWorkflow { get; set; }
}
