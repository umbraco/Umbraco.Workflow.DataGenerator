namespace Umbraco.Workflow.DataGenerator.Configuration;

public interface IWorkflowDestructor
{
    /// <summary>
    /// Proceed with the greatest of caution - deletes all Workflow data except the settings table
    /// </summary>
    /// <returns></returns>
    Task Armageddon();
}
