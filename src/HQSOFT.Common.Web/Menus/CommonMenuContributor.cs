using HQSOFT.Common.Permissions;
using HQSOFT.Common.Localization;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Volo.Abp.Authorization.Permissions;

namespace HQSOFT.Common.Web.Menus;

public class CommonMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return;
        }

        var moduleMenu = AddModuleMenuItem(context); //Do not delete `moduleMenu` variable as it will be used by ABP Suite!

        AddMenuItemComments(context, moduleMenu);

        AddMenuItemNotifications(context, moduleMenu);
    }

    private static ApplicationMenuItem AddModuleMenuItem(MenuConfigurationContext context)
    {
        var moduleMenu = new ApplicationMenuItem(
            CommonMenus.Prefix,
            displayName: "Common",
            "~/Common",
            icon: "fa fa-globe");

        //Add main menu items.
        context.Menu.Items.AddIfNotContains(moduleMenu);
        return moduleMenu;
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
}