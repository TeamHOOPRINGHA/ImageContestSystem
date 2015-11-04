using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageContestSystem.Web.Models.ViewModels
{
    using System.Collections.Generic;

    public class BrowseNotificationsViewModel
    {
        public ICollection<NotificationViewModel> Notifications { get; set; } 
    }
}