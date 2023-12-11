using Microsoft.Extensions.Hosting;
using Umbraco.Cms.Core.Extensions;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;
using Umbraco.Workflow.Core.Repositories;
using Umbraco.Workflow.DataGenerator.Configuration.Workflow;

namespace Umbraco.Workflow.DataGenerator.Configuration.Umbraco;

internal sealed class UmbracoHousekeeper : IUmbracoHousekeeper
{
    private readonly IUserService _userService;
    private readonly IDataTypeService _dataTypeService;
    private readonly IContentService _contentService;
    private readonly IContentTypeService _contentTypeService;
    private readonly IMediaService _mediaService;
    private readonly IMediaTypeService _mediaTypeService;
    private readonly IDatabaseAccessor _databaseAccessor;
    private readonly IFileService _fileService;
    private readonly IMacroService _macroService;
    private readonly IHostEnvironment _hostEnvironment;

    public UmbracoHousekeeper(
        IUserService userService,
        IDataTypeService dataTypeService,
        IContentService contentService,
        IContentTypeService contentTypeService,
        IMediaTypeService mediaTypeService,
        IMediaService mediaService,
        IDatabaseAccessor databaseAccessor,
        IFileService fileService,
        IMacroService macroService,
        IHostEnvironment hostEnvironment)
    {
        _userService = userService;
        _dataTypeService = dataTypeService;
        _contentService = contentService;
        _contentTypeService = contentTypeService;
        _mediaTypeService = mediaTypeService;
        _mediaService = mediaService;
        _databaseAccessor = databaseAccessor;
        _fileService = fileService;
        _macroService = macroService;
        _hostEnvironment = hostEnvironment;
    }

    /// <inheritdoc/>
    public void RemoveExistingUsers()
    {
        IEnumerable<IUser> allUsers = _userService.GetAll(1, 1000, out long _);
        foreach (IUser user in allUsers)
        {
            if (user.IsAdmin() || !user.Email.EndsWith(WorkflowConfigurator.EmailDomain))
            {
                continue;
            }

            _userService.Delete(user);
        }
    }

    /// <inheritdoc/>
    public void RemoveNonCoreDatatypes()
    {
        IEnumerable<IDataType> dataTypes = _dataTypeService.GetAll();
        foreach (IDataType dataType in dataTypes)
        {
            if (DataTypeExtensions.IsBuildInDataType(dataType.Key))
            {
                continue;
            }

            _dataTypeService.Delete(dataType);
        }
    }

    /// <inheritdoc/>
    public async Task RemoveExistingContent()
    {
        IEnumerable<IContentType> contentTypes = _contentTypeService.GetAll();
        _contentService.DeleteOfTypes(contentTypes.Select(x => x.Id));

        foreach (IContentType contentType in contentTypes)
        {
            _contentTypeService.Delete(contentType);
        }

        // Can't find an API for deleting content type folders...
        // Once we get here, we've deleted all content and content types,
        // so safe (enough) to delete the folders by type
        await _databaseAccessor.Execute($"DELETE FROM [umbracoNode] WHERE [umbracoNode].[nodeObjectType] = @0", Cms.Core.Constants.ObjectTypes.DocumentTypeContainer);
    }

    /// <inheritdoc/>
    public void RemoveExistingMedia()
    {
        IEnumerable<IMediaType> mediaTypes = _mediaTypeService.GetAll();
        _mediaService.DeleteMediaOfTypes(mediaTypes.Select(x => x.Id));

        foreach (IMediaType mediaType in mediaTypes)
        {
            if (mediaType.IsSystemMediaType())
            {
                continue;
            }

            _mediaTypeService.Delete(mediaType);
        }
    }

    /// <inheritdoc/>
    public void RemoveExistingFileAssets()
    {
        foreach (ITemplate template in _fileService.GetTemplates())
        {
            _fileService.DeleteTemplate(template.Alias);
        }

        foreach (IPartialView partial in _fileService.GetPartialViews())
        {
            if (partial.Path.Contains("UmbracoWorkflow"))
            {
                continue;
            }

            _ = _fileService.DeletePartialView(partial.Path);
        }

        foreach (IStylesheet stylesheet in _fileService.GetStylesheets())
        {
            _fileService.DeleteStylesheet(stylesheet.Path);
        }

        foreach (IScript script in _fileService.GetScripts())
        {
            _fileService.DeleteScript(script.Path);
        }

        // clean up any empty directories
        string partialRoot = _hostEnvironment.MapPathContentRoot(Cms.Core.Constants.SystemDirectories.PartialViews);
        DeleteDirectories(new DirectoryInfo(partialRoot));
    }

    /// <inheritdoc/>
    public void RemoveExistingMacros()
    {
        foreach (IMacro macro in _macroService.GetAll())
        {
            _macroService.Delete(macro);
        }

        // macro partials is a bit messy - need the path to the file to delete it.
        string macroRoot = _hostEnvironment.MapPathContentRoot(Cms.Core.Constants.SystemDirectories.MacroPartials);
        DeleteDirectories(new DirectoryInfo(macroRoot), force: true);
    }

    private void DeleteDirectories(DirectoryInfo directory, bool deleteSelf = false, bool force = false)
    {
        FileInfo[] files = directory.GetFiles();

        if (files.Length != 0 && !force)
        {
            return;
        }

        DirectoryInfo[] subDirectories = directory.GetDirectories();
        foreach (DirectoryInfo subDirectory in subDirectories)
        {
            DeleteDirectories(subDirectory, true);
        }

        if (files.Length > 0 && force)
        {
            foreach (FileInfo file in files)
            {
                file.Delete();
            }
        }

        if (subDirectories.Length == 0 && deleteSelf)
        {
            directory.Delete();
        }
    }
}
