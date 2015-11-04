using ImageContestSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ImageContestSystem.Web.Models.ViewModels
{
    public class NotificationViewModel
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public string SenderId { get; set; }

        public string SenderName { get; set; }

        public static Expression<Func<Notification, NotificationViewModel>> Create
        {
            get
            {
                return n => new NotificationViewModel
                {
                    Id = n.Id,
                    Message = n.Text,
                    SenderId = n.SenderId,
                    SenderName = n.Sender.UserName
                };
            }
        }
    }
}