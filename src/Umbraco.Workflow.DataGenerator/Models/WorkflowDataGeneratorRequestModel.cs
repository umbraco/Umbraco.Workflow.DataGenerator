namespace Umbraco.Workflow.DataGenerator.Models;

public sealed class WorkflowDataGeneratorRequestModel
{
    public int UserCount { get; set; }

    public int GroupCount { get; set; }

    public int UsersPerGroup { get; set; }

    public int GroupsPerWorkflow { get; set; }
}
