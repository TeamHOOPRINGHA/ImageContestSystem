namespace ImageContestSystem.Web.Models.ViewModels
{
    using System.Collections.Generic;

    public class HomeViewModel
    {
        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<ContestParticipantViewModel> Contests { get; set; }
    }
}