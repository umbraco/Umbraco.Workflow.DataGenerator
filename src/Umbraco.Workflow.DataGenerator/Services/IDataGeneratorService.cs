using Umbraco.Cms.Core.Packaging;
using Umbraco.Workflow.DataGenerator.Models;

namespace Umbraco.Workflow.DataGenerator.Services;

public interface IDataGeneratorService
{
    /// <summary>
    /// The main entry point for generating test data.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<InstallationSummary?> TryGenerate(WorkflowDataGeneratorRequestModel model);

    /// <summary>
    /// Gets the current status for the Umbraco instance.
    /// </summary>
    /// <returns></returns>
    GeneratorStatusModel GetStatus();

    /// <summary>
    /// Sets the status to indicate the tour should not be shown.
    /// </summary>
    void DismissTour();

    /// <summary>
    /// Deletes everything - users, content, content types, and all workflow configuration.
    /// </summary>
    /// <returns></returns>
    Task Reset(GeneratorStatusModel currentStatus, GeneratorStatusModel futureStatus);
}
