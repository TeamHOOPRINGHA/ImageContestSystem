namespace ImageContestSystem.Web.Controllers
{
    using Models.ViewModels;
    using Microsoft.AspNet.Identity;
    using System.Linq;
    using System.Web.Mvc;

    public class UserController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult ViewProfile()
        {
            var loggedUserId = this.User.Identity.GetUserId();

            var userProfile = this.Data.Users.All()
                .Where(u => u.Id == loggedUserId)
                .Select(UserProfileViewModel.Create)
                .FirstOrDefault();
            
            return View(userProfile);
        }
    }
}