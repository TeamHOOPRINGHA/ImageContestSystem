namespace ImageContestSystem.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Data.Models;
    using Models.ViewModels;

    public class PhotoController : BaseController
    {
        [Authorize]
        [HttpPost]
        public ActionResult Add(PhotoViewModel model)
        {
            if (!this.ModelState.IsValid || model == null)
            {
                return RedirectToAction("Index", "Contest");
            }

            var loggedUserId = this.User.Identity.GetUserId();
            var contest = this.Data.Contests.Find(model.ContestId);

            var photo = new Picture()
            {
                AuthorId = loggedUserId,
                ContestId = model.ContestId,
                CreatedOn = DateTime.Now,
                LocationPath = model.Location
            };

            this.Data.Pictures.Add(photo);
            contest.Pictures.Add(photo);
            this.Data.SaveChanges();

            return RedirectToAction("Index", "Contest");
        }
    }
}