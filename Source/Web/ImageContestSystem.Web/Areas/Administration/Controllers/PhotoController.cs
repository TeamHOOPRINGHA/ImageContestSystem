namespace ImageContestSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using ImageContestSystem.Web.Controllers;
    using ImageContestSystem.Web.Models.ViewModels;
    using ImageContestSystem.Data.UnitOfWork;

    [Authorize(Roles = "Administrator")]
    public class PhotoController : BaseController
    {
        public PhotoController(IImageContestData data)
            : base(data)
        {
        }

        public ActionResult Get()
        {
            var photos = this.Data.Pictures.All()
                .OrderByDescending(p => p.CreatedOn)
                .Select(p => new PhotoViewModel
                {
                    Id = p.Id,
                    Location = p.LocationPath,
                    Author = p.Author.UserName,
                    Votes = p.Votes.Count,
                    IsDeleted = p.IsDeleted
                })
                .ToList();

            return this.PartialView("_ShowPhotosPartial", photos);
        }

        public ActionResult Remove(int id)
        {
            var removedPhoto = this.Data.Pictures.Find(id);
            removedPhoto.IsDeleted = true;
            this.Data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
	}
}