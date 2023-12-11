namespace Umbraco.Workflow.DataGenerator.Configuration.Umbraco;

public interface IUmbracoHousekeeper
{
    /// <summary>
    /// Removes all users where the assigned email address does not match
    /// the domain assigend by the WorkflowConfigurator.
    /// </summary>
    void RemoveExistingUsers();

    /// <summary>
    /// Removes all non-core datatypes (ie anything added by a package installation).
    /// </summary>
    void RemoveNonCoreDatatypes();

    /// <summary>
    /// Removes all existing content and content types.
    /// </summary>
    Task RemoveExistingContent();

    /// <summary>
    /// Removes all existing media items.
    /// </summary>
    void RemoveExistingMedia();

    /// <summary>
    /// Removes all existing views - templates, partials, stylesheets, scripts etc.
    /// </summary>
    void RemoveExistingFileAssets();

    /// <summary>
    /// Removes all existing macros (yuck).
    /// </summary>
    void RemoveExistingMacros();
}
