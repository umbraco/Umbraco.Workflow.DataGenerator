using System.IO.Compression;
using System.Xml.Linq;
using Umbraco.Cms.Core.Packaging;

namespace Umbraco.Workflow.DataGenerator.Configuration.Umbraco;

public interface IPackageInstaller
{
    /// <summary>
    /// Attempts to install the package provided in the xml document.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="zipPackage"></param>
    /// <returns></returns>
    InstallationSummary Install(XDocument xml, ZipArchive? zipPackage);

    /// <summary>
    /// Publishes all root content branches.s
    /// </summary>
    void Publish();

    /// <summary>
    /// Attempts to load the package.zip from the starter kit repository.
    /// </summary>
    /// <returns></returns>
    Task<(XDocument? PackageXml, ZipArchive? ZipArchive)> TryGetRemotePackage();
}
