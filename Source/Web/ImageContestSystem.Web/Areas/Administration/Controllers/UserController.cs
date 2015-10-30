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
    public class UserController : BaseController
    {
        public ActionResult Get()
        {
            var users = this.Data.Users.All()
                .Take(10)
                .Select(UserProfileViewModel.Create)
                .ToList();

            return this.PartialView("_ShowUsersPartial", users);
        }
	}
}