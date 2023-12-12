using Umbraco.Workflow.DataGenerator.Models;

namespace Umbraco.Workflow.DataGenerator.Services;

public interface IDataGeneratorService
{
    /// <summary>
    /// The main entry point for generating test data.
    /// </summary>
    /// <param name="model"></param>
    Task<bool> TryGenerate(WorkflowDataGeneratorRequestModel model);

    /// <summary>
    /// Gets the current status for the Umbraco instance.
    /// </summary>
    /// <returns></returns>
    bool GetStatus();

    /// <summary>
    /// Deletes all workflow configuration.
    /// </summary>
    /// <returns></returns>
    Task Reset();
}
