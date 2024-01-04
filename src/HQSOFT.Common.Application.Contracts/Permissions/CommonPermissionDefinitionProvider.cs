using HQSOFT.Common.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace HQSOFT.Common.Permissions;

public class CommonPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CommonPermissions.GroupName, L("Permission:Common"));

        var testCommonPermission = myGroup.AddPermission(CommonPermissions.TestCommons.Default, L("Permission:TestCommons"));
        testCommonPermission.AddChild(CommonPermissions.TestCommons.Create, L("Permission:Create"));
        testCommonPermission.AddChild(CommonPermissions.TestCommons.Edit, L("Permission:Edit"));
        testCommonPermission.AddChild(CommonPermissions.TestCommons.Delete, L("Permission:Delete"));

        var assignedToPermission = myGroup.AddPermission(CommonPermissions.AssignedTos.Default, L("Permission:AssignedTos"));
        assignedToPermission.AddChild(CommonPermissions.AssignedTos.Create, L("Permission:Create"));
        assignedToPermission.AddChild(CommonPermissions.AssignedTos.Edit, L("Permission:Edit"));
        assignedToPermission.AddChild(CommonPermissions.AssignedTos.Delete, L("Permission:Delete"));

        var taskAssignmentPermission = myGroup.AddPermission(CommonPermissions.TaskAssignments.Default, L("Permission:TaskAssignments"));
        taskAssignmentPermission.AddChild(CommonPermissions.TaskAssignments.Create, L("Permission:Create"));
        taskAssignmentPermission.AddChild(CommonPermissions.TaskAssignments.Edit, L("Permission:Edit"));
        taskAssignmentPermission.AddChild(CommonPermissions.TaskAssignments.Delete, L("Permission:Delete"));

        var shareWithPermission = myGroup.AddPermission(CommonPermissions.ShareWiths.Default, L("Permission:ShareWiths"));
        shareWithPermission.AddChild(CommonPermissions.ShareWiths.Create, L("Permission:Create"));
        shareWithPermission.AddChild(CommonPermissions.ShareWiths.Edit, L("Permission:Edit"));
        shareWithPermission.AddChild(CommonPermissions.ShareWiths.Delete, L("Permission:Delete"));

        var notificationPermission = myGroup.AddPermission(CommonPermissions.Notifications.Default, L("Permission:Notifications"));
        notificationPermission.AddChild(CommonPermissions.Notifications.Create, L("Permission:Create"));
        notificationPermission.AddChild(CommonPermissions.Notifications.Edit, L("Permission:Edit"));
        notificationPermission.AddChild(CommonPermissions.Notifications.Delete, L("Permission:Delete"));

        var commentPermission = myGroup.AddPermission(CommonPermissions.Comments.Default, L("Permission:Comments"));
        commentPermission.AddChild(CommonPermissions.Comments.Create, L("Permission:Create"));
        commentPermission.AddChild(CommonPermissions.Comments.Edit, L("Permission:Edit"));
        commentPermission.AddChild(CommonPermissions.Comments.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CommonResource>(name);
    }
}