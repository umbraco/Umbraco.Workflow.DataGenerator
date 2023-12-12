using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Workflow.Core.Models;
using Umbraco.Workflow.Core.Models.Pocos;
using Umbraco.Workflow.Core.Models.ViewModels;
using Umbraco.Workflow.Core.Services;

namespace Umbraco.Workflow.DataGenerator.Configuration;

internal sealed class WorkflowConfigurator : IWorkflowConfigurator
{
    private readonly IGroupService _groupService;
    private readonly IConfigService _configService;
    private readonly IUserService _userService;
    private readonly IContentService _contentService;
    private readonly IBackOfficeUserManager _userManager;
    private readonly IReadOnlyUserGroup? _editorGroup;
    private readonly GlobalSettings _globalSettings;
    private readonly IKeyValueService _keyValueService;

    private readonly string _defaultVariant;
    public static readonly string EmailDomain = "@umbraco.workflow";

    public WorkflowConfigurator(
        IOptionsSnapshot<GlobalSettings> globalSettings,
        IUserService userService,
        IGroupService groupService,
        IBackOfficeUserManager userManager,
        IContentService contentService,
        IConfigService configService,
        IDefaultCultureAccessor defaultCultureAccessor,
        IKeyValueService keyValueService)
    {
        _userService = userService;
        _groupService = groupService;
        _userManager = userManager;
        _contentService = contentService;
        _configService = configService;
        _globalSettings = globalSettings.Value;
        _defaultVariant = defaultCultureAccessor.DefaultCulture;
        _keyValueService = keyValueService;

        _editorGroup = _userService.GetUserGroupByAlias("editor") as IReadOnlyUserGroup ?? throw new NullReferenceException(nameof(_editorGroup));
    }

    /// <inheritdoc/>
    public async Task TryAssignGroupPermissions(IContent content, List<int> groupIds, int groupsPerWorkflow)
    {
        IEnumerable<int> groupsToAssign = Enumerable.Empty<int>();
        Random r = new();

        // if 0, get random number of groups
        if (groupsPerWorkflow != 0)
        {
            groupsToAssign = groupIds.OrderBy(x => r.Next()).Take(groupsPerWorkflow);
        }
        else
        {
            int i = r.Next(1, Math.Min(2, groupIds.Count));
            groupsToAssign = groupIds.OrderBy(x => r.Next()).Take(i);
        }

        ConfigModel config = new()
        {
            Id = content.Id,
            Permissions = groupsToAssign.Select((g, i) => new UserGroupPermissionsPoco()
            {
                Variant = _defaultVariant,
                NodeId = content.Id,
                GroupId = g,
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
    public async Task<int?> TryCreateApprovalGroups(int i, List<int> userIds, int usersPerGroup)
    {
        string alias = $"group-{i}";
        string name = $"Group {i}";

        (OperationResult result, UserGroupViewModel group) = await _groupService.CreateUserGroupAsync(new()
        {
            Name = name,
            Alias = alias,
        });

        if (result.Success == false)
        {
            return null;
        }

        IEnumerable<int> usersToAssign = Enumerable.Empty<int>();
        Random r = new();

        // if 0, get random number of users
        if (usersPerGroup != 0)
        {
            usersToAssign = userIds.OrderBy(x => r.Next()).Take(usersPerGroup);
        }
        else
        {
            int userCount = r.Next(1, Math.Min(1, userIds.Count));
            usersToAssign = userIds.OrderBy(x => r.Next()).Take(userCount);
        }

        group.Users = usersToAssign
            .Select(id => new User2UserGroupViewModel() { GroupId = group.GroupId, UserId = id })
            .ToList();

        _ = await _groupService.UpdateUserGroupAsync(group);

        return group.GroupId;
    }

    /// <inheritdoc/>
    public async Task<int?> TryCreateUser(int i)
    {
        string username = $"user-{i}{EmailDomain}";

        IUser? existingUser = _userService.GetByUsername(username);

        // we don't want to recreate existing users
        if (existingUser is not null)
        {
            existingUser.IsApproved = true;
            existingUser.IsLockedOut = false;
            existingUser.AddGroup(_editorGroup!);

            _userService.Save(existingUser);
            return existingUser.Id;
        }

        var identityUser = BackOfficeIdentityUser.CreateNew(_globalSettings, username, username, _globalSettings.DefaultUILanguage);
        identityUser.Name = username;

        IdentityResult created = await _userManager.CreateAsync(identityUser);
        if (created.Succeeded == false)
        {
            return null;
        }

        IdentityResult result = await _userManager.AddPasswordAsync(identityUser, username);
        if (result.Succeeded == false)
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
        user.AddGroup(_editorGroup!);

        _userService.Save(user);
        return user.Id;
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
