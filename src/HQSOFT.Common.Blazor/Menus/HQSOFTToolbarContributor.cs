using DevExpress.Blazor.Navigation.Internal;
using HQSOFT.Common.Blazor.Pages.Component;
using HQSOFT.Common.Localization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.UI.Navigation;

namespace HQSOFT.Common.Blazor.Menus
{ 
    public class HQSOFTToolbarContributor : IToolbarContributor
    {
        public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
        {
            if (context.Toolbar.Name == StandardToolbars.Main)
            {
                context.Toolbar.Items.Insert(0, new ToolbarItem(typeof(HQSOFTNotifications)));
            }

            return Task.CompletedTask;
        }
    }
     
}
