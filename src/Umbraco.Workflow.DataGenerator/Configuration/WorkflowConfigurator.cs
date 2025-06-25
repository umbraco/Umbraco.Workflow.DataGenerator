using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;
using Umbraco.Workflow.Core.ApprovalGroups.Services;
using Umbraco.Workflow.Core.ApprovalGroups.ViewModels;
using Umbraco.Workflow.Core.Services;

namespace Umbraco.Workflow.DataGenerator.Configuration;

internal sealed class WorkflowConfigurator : IWorkflowConfigurator
{
    public static readonly string EmailDomain = "@umbraco.workflow";

    private readonly IGroupService _groupService;
    private readonly IConfigService _configService;
    private readonly IUserService _userService;
    private readonly IContentService _contentService;
    private readonly IBackOfficeUserManager _userManager;
    private readonly GlobalSettings _globalSettings;
    private readonly IKeyValueService _keyValueService;
    private readonly IUserGroupService _userGroupService;

    private readonly string _defaultVariant;

    public WorkflowConfigurator(
        IOptionsSnapshot<GlobalSettings> globalSettings,
        IUserService userService,
        IGroupService groupService,
        IBackOfficeUserManager userManager,
        IContentService contentService,
        IConfigService configService,
        IDefaultCultureAccessor defaultCultureAccessor,
        IKeyValueService keyValueService,
        IUserGroupService userGroupService)
    {
        _userService = userService;
        _groupService = groupService;
        _userManager = userManager;
        _contentService = contentService;
        _configService = configService;
        _globalSettings = globalSettings.Value;
        _defaultVariant = defaultCultureAccessor.DefaultCulture;
        _keyValueService = keyValueService;
        _userGroupService = userGroupService;
    }

    /// <inheritdoc/>
    public async Task TryAssignGroupPermissions(IContent content, IEnumerable<Guid> groupIds, int groupsPerWorkflow)
    {
        IEnumerable<Guid> groupsToAssign = [];
        Random r = new();

        // if 0, get random number of groups
        if (groupsPerWorkflow != 0)
        {
            groupsToAssign = groupIds.OrderBy(x => r.Next()).Take(groupsPerWorkflow);
        }
        else
        {
            int i = r.Next(1, Math.Min(2, groupIds.Count()));
            groupsToAssign = groupIds.OrderBy(x => r.Next()).Take(i);
        }

        DocumentConfigUpdateRequestModel config = new()
        {
            Key = content.Key,
            Variant = _defaultVariant,
            Permissions = groupsToAssign.Select((g, i) => new ApprovalGroupDetailPermissionConfigModel()
            {
                GroupUnique = g,
                Permission = i,
            }),
        };

        _ = await _configService.UpdateNodeConfig(config);

        if (content.Level != 1)
        {
            return;
        }

        foreach (IContent node in _contentService.GetPagedChildren(content.Id, 0, 100, out _))
        {
            await TryAssignGroupPermissions(node, groupIds, groupsPerWorkflow);
        }
    }

    /// <inheritdoc/>
    public async Task<Guid?> TryCreateApprovalGroups(int i, IEnumerable<Guid> userIds, int usersPerGroup)
    {
        string alias = $"group-{i}";
        string name = $"Group {i}";

        Attempt<ApprovalGroupDetailResponseModel, ApprovalGroupOperationStatus> result = await _groupService.CreateUserGroupAsync(new()
        {
            Unique = Guid.NewGuid(),
            Name = name,
            Alias = alias,
        });

        if (result.Success is false || result.Result is not ApprovalGroupDetailResponseModel group)
        {
            return null;
        }

        IEnumerable<Guid> usersToAssign = [];
        Random r = new();

        // if 0, get random number of users
        if (usersPerGroup != 0)
        {
            usersToAssign = userIds.OrderBy(x => r.Next()).Take(usersPerGroup);
        }
        else
        {
            int userCount = r.Next(1, Math.Min(1, userIds.Count()));
            usersToAssign = userIds.OrderBy(x => r.Next()).Take(userCount);
        }

        group.Users = usersToAssign
            .Select(userUnique => new User2ApprovalGroupViewModel() { GroupUnique = group.Unique, UserUnique = userUnique })
            .ToList();

        _ = await _groupService.UpdateUserGroupAsync(group);

        return group.Unique;
    }

    /// <inheritdoc/>
    public async Task<Guid?> TryCreateUser(int i)
    {
        string username = $"user-{i}{EmailDomain}";

        IUser? existingUser = _userService.GetByUsername(username);
        IUserGroup? editorGroup = await _userGroupService.GetAsync(Constants.Security.EditorGroupKey)
            ?? throw new NullReferenceException("The 'editors' user group does not exist. Please create it before running the data generator.");

        // we don't want to recreate existing users
        if (existingUser is not null)
        {
            existingUser.IsApproved = true;
            existingUser.IsLockedOut = false;
            existingUser.AddGroup(editorGroup.ToReadOnlyGroup());

            _userService.Save(existingUser);
            return existingUser.Key;
        }

        var identityUser = BackOfficeIdentityUser.CreateNew(_globalSettings, username, username, _globalSettings.DefaultUILanguage);
        identityUser.Name = username;

        IdentityResult created = await _userManager.CreateAsync(identityUser);
        if (created.Succeeded is false)
        {
            return null;
        }

        IdentityResult result = await _userManager.AddPasswordAsync(identityUser, username);
        if (result.Succeeded is false)
        {
            return null;
        }

        // user exists, fetch it to confirm
        // username == email
        IUser? user = _userService.GetByEmail(username);
        if (user is null)
        {
            return null;
        }

        user.IsApproved = true;
        user.IsLockedOut = false;
        user.AddGroup(editorGroup.ToReadOnlyGroup());

        _userService.Save(user);
        return user.Key;
    }

    public void SetStatus(bool generated) => _keyValueService.SetValue(GetType().FullName!, generated.ToString());

    public bool GetStatus()
    {
        string? status = _keyValueService.GetValue(GetType().FullName!);
        if (status is null || !bool.TryParse(status, out bool result))
        {
            return false;
        }

        return result;
    }
}
