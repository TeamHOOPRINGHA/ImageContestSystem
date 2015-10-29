namespace ImageContestSystem.Web.Models.ViewModels
{
    public class ContestParticipantViewModel : ContestViewModel
    {
        public string Creator { get; set; }

        public int Pictures { get; set; }

        public bool HasAddedPhoto { get; set; }
    }
}