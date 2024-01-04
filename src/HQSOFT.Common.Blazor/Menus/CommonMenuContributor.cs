using HQSOFT.Common.Permissions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using HQSOFT.Common.Localization;
using Volo.Abp.UI.Navigation;

namespace HQSOFT.Common.Blazor.Menus;

public class CommonMenuContributor : IMenuContributor
{

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {

        var moduleMenu = AddModuleMenuItem(context);
        AddMenuItemTestCommons(context, moduleMenu);

        AddMenuItemTaskAssignments(context, moduleMenu);

        AddMenuItemShareWiths(context, moduleMenu);

        AddMenuItemNotifications(context, moduleMenu);

        AddMenuItemComments(context, moduleMenu);
    }

    private static ApplicationMenuItem AddModuleMenuItem(MenuConfigurationContext context)
    {
        var moduleMenu = new ApplicationMenuItem(
            CommonMenus.Prefix,
            context.GetLocalizer<CommonResource>()["Menu:Common"],
            icon: "fa fa-folder"
        );

        context.Menu.Items.AddIfNotContains(moduleMenu);
        return moduleMenu;
    }
    private static void AddMenuItemTestCommons(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.CommonMenus.TestCommons,
                context.GetLocalizer<CommonResource>()["Menu:TestCommons"],
                "/Common/TestCommons",
                icon: "fa fa-file-alt",
                requiredPermissionName: CommonPermissions.TestCommons.Default
            )
        );
    }

    private static void AddMenuItemTaskAssignments(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.CommonMenus.TaskAssignments,
                context.GetLocalizer<CommonResource>()["Menu:TaskAssignments"],
                "/Common/TaskAssignments",
                icon: "fa fa-file-alt",
                requiredPermissionName: CommonPermissions.TaskAssignments.Default
            )
        );
    }

    private static void AddMenuItemShareWiths(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.CommonMenus.ShareWiths,
                context.GetLocalizer<CommonResource>()["Menu:ShareWiths"],
                "/Common/ShareWiths",
                icon: "fa fa-file-alt",
                requiredPermissionName: CommonPermissions.ShareWiths.Default
            )
        );
    }

    private static void AddMenuItemNotifications(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.CommonMenus.Notifications,
                context.GetLocalizer<CommonResource>()["Menu:Notifications"],
                "/Common/Notifications",
                icon: "fa fa-file-alt",
                requiredPermissionName: CommonPermissions.Notifications.Default
            )
        );
    }

    private static void AddMenuItemComments(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.CommonMenus.Comments,
                context.GetLocalizer<CommonResource>()["Menu:Comments"],
                "/Common/Comments",
                icon: "fa fa-file-alt",
                requiredPermissionName: CommonPermissions.Comments.Default
            )
        );
    }
}