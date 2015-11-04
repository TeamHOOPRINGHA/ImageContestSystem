using ImageContestSystem.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageContestSystem.Web.Areas.Administration.Models
{
    public class UserViewModel
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public int OwnContest { get; set; }

        public int ParticipatedInContests { get; set; }

        public int WonContests { get; set; }

        public int UploadedPhotos { get; set; }

        public ICollection<ContestViewModel> Contests { get; set; }

        public ICollection<ContestViewModel> OwnContests { get; set; } 
    }
}