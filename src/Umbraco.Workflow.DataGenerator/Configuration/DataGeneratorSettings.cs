using Umbraco.Cms.Core.Configuration.Models;

namespace Umbraco.Workflow.DataGenerator.Configuration;

[UmbracoOptions(OptionsName)]
public class DataGeneratorSettings
{
    public const string OptionsName = "Umbraco.WorkflowDataGenerator";

    /// <summary>
    /// Gets or sets a URL for getting a site package for use in data generation.
    /// </summary>
    public string? PackageZipSource { get; set; } = "https://github.com/umbraco/The-Starter-Kit/raw/v11/dev/src/Umbraco.SampleSite/Migrations/package.zip";
}
