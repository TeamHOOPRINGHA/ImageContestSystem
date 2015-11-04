namespace ImageContestSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using ImageContestSystem.Web.Controllers;
    using ImageContestSystem.Web.Models.ViewModels;
    using ImageContestSystem.Data.UnitOfWork;

    [Authorize(Roles = "Administrator")]
    public class ContestController : BaseController
    {
        public ContestController(IImageContestData data)
            : base(data)
        {
        }

        public ActionResult Get()
        {
            var contests = this.Data.Contests.All()
                .OrderByDescending(c => c.CreatedOn)
                .ProjectTo<ContestViewModel>()
                .ToList();

            return this.PartialView("_ShowContestsPartial", contests);
        }

        public ActionResult DismissContest(int id)
        {
            var contestDismissed = this.Data.Contests.Find(id);
            contestDismissed.IsDismissed = true;
            this.Data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult DeleteContest(int id)
        {
            var contestDeleted = this.Data.Contests.Find(id);
            this.Data.Contests.Delete(contestDeleted);
            this.Data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
	}
}