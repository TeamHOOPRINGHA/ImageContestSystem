namespace ImageContestSystem.Web.Controllers
{
    using Models.ViewModels;
    using Microsoft.AspNet.Identity;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web;
    using System.Net;
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
        [HttpPost]
        public ActionResult InviteToContest(UserInvitationViewModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var contest = this.Data.Contests.Find(model.ContestId);

            if (!this.ModelState.IsValid)
            {
                throw new HttpException();
            }
            
            if (loggedUserId != contest.CreatorId)
            {
                throw new HttpException();
            }

            var invitedUser = this.Data.Users.All().FirstOrDefault(u => u.Id == model.UserId);
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

        public ActionResult SearchUser(string username, int contestId)
        {
            username = username.ToLower();
            var model = new SearchViewModel
            {
                Users = this.Data.Users.All()
                    .Where(u => u.UserName.ToLower().Contains(username))
                    .Select(u => new UserInvitationViewModel
                    {
                        ContestId = contestId,
                        UserId = u.Id,
                        Username = u.UserName
                    })
                    .ToList()
            };

            return this.PartialView("_UserSearchPartial", model);
        }
    }
}