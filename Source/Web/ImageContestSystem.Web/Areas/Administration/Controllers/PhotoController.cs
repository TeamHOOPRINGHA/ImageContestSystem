namespace ImageContestSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using ImageContestSystem.Web.Controllers;
    using ImageContestSystem.Web.Models.ViewModels;

    [Authorize(Roles = "Administrator")]
    public class PhotoController : BaseController
    {
        public ActionResult Get()
        {
            var photos = this.Data.Pictures.All()
                .Take(10)
                .Select(p => new PhotoViewModel
                {
                    Location = p.LocationPath,
                    Author = p.Author.UserName
                })
                .ToList();

            return this.PartialView("_ShowPhotosPartial", photos);
        }
	}
}