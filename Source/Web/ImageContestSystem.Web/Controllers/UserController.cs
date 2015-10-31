namespace ImageContestSystem.Web.Controllers
{
    using Models.ViewModels;
    using Microsoft.AspNet.Identity;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web;
    using AutoMapper.QueryableExtensions;

    public class UserController : BaseController
    {
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

        [Authorize]
        public ActionResult ApplyToContest(int id)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.Find(loggedUserId);
            var contest = this.Data.Contests.Find(id);

            if (contest.Participants.Any(p => p.Id == loggedUserId))
            {
                throw new HttpException();
            }

            contest.Participants.Add(loggedUser);
            this.Data.SaveChanges();

            return RedirectToAction("Index", "Contest");
        }

        [Authorize]
        public ActionResult InviteToContest(string invitedUsername, int contestId)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var contest = this.Data.Contests.Find(contestId);
            
            if (loggedUserId != contest.CreatorId)
            {
                throw new HttpException();
            }

            var invitedUser = this.Data.Users.All().FirstOrDefault(u => u.UserName == invitedUsername);
            if (invitedUser == null)
            {
                throw new HttpException();
            }

            invitedUser.ParticipatedIn.Add(contest);
            contest.Participants.Add(invitedUser);
            this.Data.SaveChanges();

            return RedirectToAction("View", "Contest", contest);
        }

        public ActionResult View(string username)
        {
            var user = this.Data.Users.All()
                .Where(u => u.UserName == username)
                .Select(UserProfileViewModel.Create)
                .FirstOrDefault();

            return View(user);
        }

        public ActionResult ViewAll()
        {
            var users = this.Data.Users.All()
                .Take(10)
                .Select(UserProfileViewModel.Create)
                .ToList();

            return View(users);
        }

        [Authorize]
        public ActionResult MyContests()
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var userContests = this.Data.Contests.All()
                .Where(c => c.CreatorId == loggedUserId)
                .Take(5)
                .OrderByDescending(c => c.ClosesOn)
                .ProjectTo<ContestViewModel>()
                .ToList();

            return View(userContests);
        }

        public ActionResult SearchUser(string username)
        {
            username = username.ToLower();
            var model = new SearchViewModel
            {
                Users = this.Data.Users.All()
                    .Where(u => u.UserName.ToLower().Contains(username))
                    .Select(UserProfileViewModel.Create)
                    .ToList()
            };

            return this.PartialView("_UserSearchPartial", model);
        }
    }
}