namespace ImageContestSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using ImageContestSystem.Web.Controllers;
    using ImageContestSystem.Web.Models.ViewModels;

    [Authorize(Roles = "Administrator")]
    public class ContestController : BaseController
    {
        public ActionResult Get()
        {
            var contests = this.Data.Contests.All()
                .Where(c => c.ClosesOn > DateTime.Now)
                .OrderByDescending(c => c.ClosesOn)
                .Take(10)
                .ProjectTo<ContestViewModel>()
                .ToList();

            return this.PartialView("_ShowContestsPartial", contests);
        }
	}
}