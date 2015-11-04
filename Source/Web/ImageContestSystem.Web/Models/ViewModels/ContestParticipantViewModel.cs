using System.Collections.Generic;
namespace ImageContestSystem.Web.Models.ViewModels
{
    public class ContestParticipantViewModel : ContestViewModel
    {
        public string Creator { get; set; }

        public string CurrentLeader { get; set; }

        public ICollection<PhotoViewModel> Pictures { get; set; }

        public bool HasAddedPhoto { get; set; }
    }
}