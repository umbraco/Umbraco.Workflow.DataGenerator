using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Workflow.DataGenerator.Configuration;
using Umbraco.Workflow.DataGenerator.Models;

namespace Umbraco.Workflow.DataGenerator.Services;

internal sealed class DataGeneratorService : IDataGeneratorService
{
    private readonly IWorkflowConfigurator _workflowConfigurator;
    private readonly IContentService _contentService;
    private readonly IWorkflowDestructor _workflowDestructor;

    public DataGeneratorService(
        IContentService contentService,
        IWorkflowConfigurator workflowConfigurator,
        IWorkflowDestructor workflowDestructor)
    {
        _contentService = contentService;
        _workflowConfigurator = workflowConfigurator;
        _workflowDestructor = workflowDestructor;
    }

    public async Task Reset()
    {
        await _workflowDestructor.Armageddon();
        _workflowConfigurator.SetStatus(false);
    }

    /// <inheritdoc/>
    public bool GetStatus() => _workflowConfigurator.GetStatus();

    /// <inheritdoc/>
    public async Task<bool> TryGenerate(WorkflowDataGeneratorRequestModel model)
    {
        bool hasData = _workflowConfigurator.GetStatus();

        if (hasData)
        {
            await _workflowDestructor.Armageddon();
        }

        bool result = await ConfigureWorkflow(model);
        _workflowConfigurator.SetStatus(result);

        return result;
    }

    private async Task<bool> ConfigureWorkflow(WorkflowDataGeneratorRequestModel model)
    {
        List<Guid> groupIds = [];
        List<Guid> userIds = [];

        for (var i = 1; i <= model.UserCount; i += 1)
        {
            if (await _workflowConfigurator.TryCreateUser(i) is Guid userId)
            {
                userIds.Add(userId);
            }
        }

        if (userIds.Count == 0)
        {
            return false;
        }

        for (var i = 1; i <= model.GroupCount; i += 1)
        {
            if (await _workflowConfigurator.TryCreateApprovalGroups(i, userIds, model.UsersPerGroup) is Guid groupId)
            {
                groupIds.Add(groupId);
            }
        }

        if (groupIds.Count == 0)
        {
            return false;
        }

        foreach (IContent root in _contentService.GetRootContent())
        {
            await _workflowConfigurator.TryAssignGroupPermissions(root, groupIds, model.GroupsPerWorkflow);
        }

        return true;
    }
}
