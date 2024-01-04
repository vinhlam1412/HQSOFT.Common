using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace HQSOFT.Common.Blazor.Pages.Component.Toolbar
{
    public class ChangeIconToggleSidebar : IScopedDependency
    {
        private bool isChanged;

        public bool IsChanged
        {
            get => isChanged;
            set
            {
                isChanged = value;
                IsChangeIcon?.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler IsChangeIcon;
    }
}
