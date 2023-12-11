using System.IO.Compression;
using System.Xml.Linq;
using Umbraco.Cms.Core.Packaging;

namespace Umbraco.Workflow.DataGenerator.Configuration.Umbraco;

public interface IMediaImporter
{
    /// <summary>
    /// Imports media items defined in the xml document,
    /// using the assets provided in the archive.
    /// </summary>
    /// <param name="installationSummary"></param>
    /// <param name="xml"></param>
    /// <param name="zipPackage"></param>
    void Import(InstallationSummary installationSummary, XDocument xml, ZipArchive zipPackage);
}
