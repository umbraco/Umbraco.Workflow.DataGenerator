using System.IO.Compression;
using System.Xml.Linq;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Packaging;
using Umbraco.Cms.Core.Services;
using Umbraco.Workflow.Core.Extensions;

namespace Umbraco.Workflow.DataGenerator.Configuration.Umbraco;

internal sealed class PackageInstaller : IPackageInstaller
{
    private readonly IPackagingService _packagingService;
    private readonly IMediaImporter _mediaImporter;
    private readonly IContentService _contentService;
    private readonly IOptions<DataGeneratorSettings> _options;
    private readonly HttpClient _httpClient;

    public PackageInstaller(
        IContentService contentService,
        IMediaImporter mediaImporter,
        IPackagingService packagingService,
        IOptions<DataGeneratorSettings> options,
        HttpClient httpClient)
    {
        _contentService = contentService;
        _mediaImporter = mediaImporter;
        _packagingService = packagingService;
        _options = options;
        _httpClient = httpClient;
    }

    /// <inheritdoc/>
    public async Task<(XDocument?, ZipArchive?)> TryGetRemotePackage()
    {
        HttpResponseMessage packageResponse = await _httpClient.GetAsync(_options.Value.PackageZipSource);
        using Stream packageZip = packageResponse.Content.ReadAsStream();
        ZipArchive zipArchive = PackageMigrationResource.GetPackageDataManifest(packageZip, out XDocument? packageXml);

        return (packageXml, zipArchive);
    }

    /// <inheritdoc/>
    public InstallationSummary Install(XDocument xml, ZipArchive? zipPackage)
    {
        InstallationSummary installationSummary = _packagingService.InstallCompiledPackageData(xml);

        if (zipPackage is null)
        {
            return installationSummary;
        }

        _mediaImporter.Import(installationSummary, xml, zipPackage);

        return installationSummary;
    }

    /// <inheritdoc/>
    public void Publish()
    {
        IEnumerable<IContent> rootContent = _contentService.GetRootContent();
        if (rootContent.IsNullOrEmpty())
        {
            return;
        }

        foreach (IContent content in rootContent)
        {
            _ = _contentService.SaveAndPublishBranch(content, true);
        }
    }
}
