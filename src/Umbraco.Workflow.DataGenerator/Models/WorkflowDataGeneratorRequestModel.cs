using System.Text.Json.Serialization;

namespace Umbraco.Workflow.DataGenerator.Models;

public sealed class WorkflowDataGeneratorRequestModel
{
    [JsonPropertyName("userCount")]
    public int UserCount { get; set; }

    [JsonPropertyName("groupCount")]
    public int GroupCount { get; set; }

    [JsonPropertyName("usersPerGroup")]
    public int UsersPerGroup { get; set; } = 0;

    [JsonPropertyName("groupsPerWorkflow")]
    public int GroupsPerWorkflow { get; set; } = 0;
}
