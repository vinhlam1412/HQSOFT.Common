using System;
using System.Collections.Generic;
using System.Text;

namespace HQSOFT.Common.Notifications
{
    public enum NotificationsType
    {
        Mention,
		Assignment,
		Alert, 
        Share
    }
    public class NotificationsTypeList
	{
        public string Value { get; set; }
        public string DisplayName { get; set; }

    }
}
