using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageContestSystem.Web.Models.ViewModels
{
    public class BrowseContestsViewModel
    {
        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<ContestViewModel> Contests { get; set; } 
    }
}