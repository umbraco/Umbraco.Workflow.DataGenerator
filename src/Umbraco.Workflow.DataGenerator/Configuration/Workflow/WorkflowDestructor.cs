using Umbraco.Workflow.Core;
using Umbraco.Workflow.Core.Repositories;

namespace Umbraco.Workflow.DataGenerator.Configuration.Workflow;

internal sealed class WorkflowDestructor : IWorkflowDestructor
{
    private readonly IDatabaseAccessor _databaseAccessor;

    public WorkflowDestructor(IDatabaseAccessor databaseAccessor) => _databaseAccessor = databaseAccessor;

    /// <inheritdoc/>
    public async Task Armageddon()
    {
        string[] deleteThese =
        [
            Constants.Tables.Instance,
            Constants.Tables.TaskInstance,
            Constants.Tables.ContentReviewNodes,
            Constants.Tables.ContentReviewConfig,
            Constants.Tables.HistoryCleanupConfig,
            Constants.Tables.TaskApprovals,
            Constants.Tables.UserGroups,
            Constants.Tables.User2UserGroup,
            Constants.Tables.Permissions,
        ];

        foreach (string tableName in deleteThese)
        {
            await _databaseAccessor.Execute($"DELETE FROM {tableName}");
        }

        // hard delete disabled users?
        //await _databaseAccessor.Execute($"DELETE FROM [umbracoUser] WHERE [umbracoUser].[userDisabled] == 1");
    }
}
