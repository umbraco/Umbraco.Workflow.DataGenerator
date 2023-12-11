using System.IO.Compression;
using System.Xml.Linq;
using System.Xml.XPath;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Packaging;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Extensions;

namespace Umbraco.Workflow.DataGenerator.Configuration.Umbraco;

internal sealed class MediaImporter : IMediaImporter
{
    private readonly IContentTypeBaseServiceProvider _contentTypeBaseServiceProvider;
    private readonly IMediaService _mediaService;
    private readonly IShortStringHelper _shortStringHelper;
    private readonly MediaFileManager _mediaFileManager;
    private readonly MediaUrlGeneratorCollection _mediaUrlGenerators;

    public MediaImporter(
        IContentTypeBaseServiceProvider contentTypeBaseServiceProvider,
        IMediaService mediaService,
        IShortStringHelper shortStringHelper,
        MediaFileManager mediaFileManager,
        MediaUrlGeneratorCollection mediaUrlGenerators)
    {
        _contentTypeBaseServiceProvider = contentTypeBaseServiceProvider;
        _mediaService = mediaService;
        _shortStringHelper = shortStringHelper;
        _mediaFileManager = mediaFileManager;
        _mediaUrlGenerators = mediaUrlGenerators;
    }

    /// <inheritdoc/>
    public void Import(InstallationSummary installationSummary, XDocument xml, ZipArchive zipPackage)
    {
        using (zipPackage)
        {
            // then we need to save each file to the saved media items
            var mediaWithFiles = xml!.XPathSelectElements(
                    "./umbPackage/MediaItems/MediaSet//*[@id][@mediaFilePath]")
                .ToDictionary(
                    x => x.AttributeValue<Guid>("key"),
                    x => x.AttributeValue<string>("mediaFilePath"));

            // Any existing media by GUID will not be installed by the package service, it will just be skipped
            // so you cannot 'update' media (or content) using a package since those are not schema type items.
            // This means you cannot 'update' the media file either. The installationSummary.MediaInstalled
            // will be empty for any existing media which means that the files will also not be updated.
            foreach (IMedia media in installationSummary.MediaInstalled)
            {
                if (mediaWithFiles.TryGetValue(media.Key, out var mediaFilePath) == false)
                {
                    continue;
                }

                // this is a media item that has a file, so find that file in the zip
                string entryPath = $"media{mediaFilePath!.EnsureStartsWith('/')}";
                ZipArchiveEntry? mediaEntry = zipPackage.GetEntry(entryPath) ?? throw new InvalidOperationException(
                        "No media file found in package zip for path " +
                        entryPath);

                // read the media file and save it to the media item
                // using the current file system provider.
                using (Stream mediaStream = mediaEntry.Open())
                {
                    media.SetValue(
                        _mediaFileManager,
                        _mediaUrlGenerators,
                        _shortStringHelper,
                        _contentTypeBaseServiceProvider,
                        Cms.Core.Constants.Conventions.Media.File,
                        Path.GetFileName(mediaFilePath)!,
                        mediaStream);
                }

                _ = _mediaService.Save(media);
            }
        }
    }
}
