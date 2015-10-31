namespace ImageContestSystem.Web.Models.ViewModels
{
    using System.Collections.Generic;

    public class SearchViewModel
    {
        public ICollection<UserProfileViewModel> Users { get; set; }
    }
}