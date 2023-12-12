using Umbraco.Cms.Core.Models;

namespace Umbraco.Workflow.DataGenerator.Configuration;

public interface IWorkflowConfigurator
{
    /// <summary>
    /// Assign groups to the root node and all child nodes. Not really random, but random enough.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="groupIds"></param>
    /// <param name="groupsPerWorkflow"></param>
    /// <returns></returns>
    Task TryAssignGroupPermissions(IContent content, List<int> groupIds, int groupsPerWorkflow);

    /// <summary>
    /// Create an approval group with a randomish number of users.
    /// </summary>
    /// <param name="i">An identifier for the group.</param>
    /// <param name="userIds">Existing user ids.</param>
    /// <param name="usersPerGroup"></param>
    /// <returns></returns>
    Task<int?> TryCreateApprovalGroups(int i, List<int> userIds, int usersPerGroup);

    /// <summary>
    /// Create a user where the username and password are the same value,
    /// formatted user-{i}@test.test
    /// Add the user to the Editors group.
    /// </summary>
    /// <param name="i"></param>
    Task<int?> TryCreateUser(int i);

    /// <summary>
    /// Sets the flag marking test data installation.
    /// </summary>
    void SetStatus(bool value);

    bool GetStatus();
}
