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

            if (contest.Participants.Contains(invitedUser))
            {
                var failMessage = string.Format("User {0} already invited", invitedUser.UserName);
                return this.Content(failMessage, "text/html");
            }

            invitedUser.ParticipatedIn.Add(contest);
            contest.Participants.Add(invitedUser);
            this.Data.SaveChanges();

            var message = string.Format("Invitation to {0} sent", invitedUser.UserName);

            return this.Content(message, "text/html");
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