using System.IO.Compression;
using System.Xml.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Packaging;
using Umbraco.Cms.Core.Services;
using Umbraco.Workflow.DataGenerator.Configuration.Umbraco;
using Umbraco.Workflow.DataGenerator.Configuration.Workflow;
using Umbraco.Workflow.DataGenerator.Models;

namespace Umbraco.Workflow.DataGenerator.Services;

internal sealed class DataGeneratorService : IDataGeneratorService
{
    private readonly IWorkflowConfigurator _workflowConfigurator;
    private readonly IContentService _contentService;
    private readonly IUmbracoHousekeeper _umbracoHousekeeper;
    private readonly IPackageInstaller _packageInstaller;
    private readonly IWorkflowDestructor _workflowDestructor;

    public DataGeneratorService(
        IContentService contentService,
        IWorkflowConfigurator workflowConfigurator,
        IUmbracoHousekeeper umbracoHousekeeper,
        IPackageInstaller packageInstaller,
        IWorkflowDestructor workflowDestructor)
    {
        _contentService = contentService;
        _workflowConfigurator = workflowConfigurator;
        _umbracoHousekeeper = umbracoHousekeeper;
        _packageInstaller = packageInstaller;
        _workflowDestructor = workflowDestructor;
    }

    public GeneratorStatusModel GetStatus() => _workflowConfigurator.GetStatus();

    public async Task Reset(GeneratorStatusModel currentStatus, GeneratorStatusModel futureStatus)
    {
        _workflowConfigurator.SetStatus(futureStatus);

        if (currentStatus.HasInstalledUmbracoData)
        {
            await CleanupUmbraco();
        }

        await _workflowDestructor.Armageddon();
    }

    /// <inheritdoc/>
    public void DismissTour()
    {
        GeneratorStatusModel status = _workflowConfigurator.GetStatus();
        status.HasDismissedTour = true;
        _workflowConfigurator.SetStatus(status);
    }

    /// <inheritdoc/>
    public async Task<InstallationSummary?> TryGenerate(WorkflowDataGeneratorRequestModel model)
    {
        // before anything happens, we need to preemptively update the status, as the installation/reset
        // will restart the application, so we lose state. We update status before any actions are taken,
        // which has the obvious risk that the status won't actually reflect the final state.
        GeneratorStatusModel currentStatus = _workflowConfigurator.GetStatus();
        GeneratorStatusModel futureStatus = new()
        {
            HasExistingUmbracoData = currentStatus.HasExistingUmbracoData,
            HasInstalledUmbracoData = !currentStatus.HasExistingUmbracoData,
            HasInstalledWorkflowData = currentStatus.WorkflowDataModel is not null,
            WorkflowDataModel = model,
        };

        _workflowConfigurator.SetStatus(futureStatus);

        // cleanup first - status is the future state
        //await Reset(currentStatus, futureStatus);

        // only install if nothing exists already
        if (!currentStatus.HasExistingUmbracoData || !currentStatus.HasInstalledUmbracoData)
        {
            (XDocument? packageXml, ZipArchive? zipArchive) = await _packageInstaller.TryGetRemotePackage();
            if (packageXml is null)
            {
                return null;
            }

            // install starter site
            _ = _packageInstaller.Install(packageXml, zipArchive);
            _packageInstaller.Publish();
        }

        // populate
        await ConfigureWorkflow(model);

        // update the status to ensure next visit in the backoffice
        // doesn't re-reun any installer steps
        futureStatus.HasInstalledWorkflowData = true;
        futureStatus.WorkflowDataModel = null;
        _workflowConfigurator.SetStatus(futureStatus);

        // do something with this to generate a useful return type
        return null;
    }

    private async Task CleanupUmbraco()
    {
        await _umbracoHousekeeper.RemoveExistingContent();

        _umbracoHousekeeper.RemoveNonCoreDatatypes();
        _umbracoHousekeeper.RemoveExistingUsers();
        _umbracoHousekeeper.RemoveExistingMedia();
        _umbracoHousekeeper.RemoveExistingFileAssets();
        _umbracoHousekeeper.RemoveExistingMacros();
    }

    private async Task ConfigureWorkflow(WorkflowDataGeneratorRequestModel model)
    {
        List<int> groupIds = [];
        List<int> userIds = [];

        for (var i = 1; i <= model.UserCount; i += 1)
        {
            if (await _workflowConfigurator.TryCreateUser(i) is int userId)
            {
                userIds.Add(userId);
            }
        }

        if (userIds.Count == 0)
        {
            return;
        }

        for (var i = 1; i <= model.GroupCount; i += 1)
        {
            if (await _workflowConfigurator.TryCreateApprovalGroups(i, userIds, model.UsersPerGroup) is int groupId)
            {
                groupIds.Add(groupId);
            }
        }

        if (groupIds.Count == 0)
        {
            return;
        }

        foreach (IContent root in _contentService.GetRootContent())
        {
            await _workflowConfigurator.TryAssignGroupPermissions(root, groupIds, model.GroupsPerWorkflow);
        }
    }
}
