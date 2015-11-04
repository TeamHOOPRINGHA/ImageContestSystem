namespace ImageContestSystem.Web.Controllers
{
    using System;
    using Models.ViewModels;
    using Microsoft.AspNet.Identity;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web;
    using System.Net;
    using AutoMapper.QueryableExtensions;
    using ImageContestSystem.Data.Models;

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

            return RedirectToAction("Index", "MyContests");
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

            if (contest.Invited.Any(p => p.Id == model.UserId) || contest.Participants.Any(p => p.Id == model.UserId))
            {
                return this.Content("User already invited", "text/html");
            }

            var invitedUser = this.Data.Users.Find(model.UserId);
            var loggedUser = this.Data.Users.Find(loggedUserId);

            invitedUser.InvitedContests.Add(contest);
            contest.Invited.Add(invitedUser);

            var text = string.Format("Hello, {0}, I'd like you to join the contest {1}", invitedUser.UserName, contest.Title);

            var notification = new Notification
             
            {
                Text = text,
                DateSent = DateTime.Now,
                ReceiverId = model.UserId,
                Receiver = invitedUser,
                SenderId = loggedUserId,
                Sender = loggedUser,
                ContestId = contest.Id
            };

            this.Data.Notifications.Add(notification);
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

        [Authorize]
        [HttpPost]
        public ActionResult AcceptInvititation(ResolveNotificationBindingModel model)
        {
            if (!this.ModelState.IsValid || model == null)
            {
                throw new HttpException();
            }

            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.Find(loggedUserId);
            var notification = this.Data.Notifications.Find(model.NotificationId);
            var contest = this.Data.Contests.Find(model.ContestId);

            contest.Invited.Remove(loggedUser);
            contest.Participants.Add(loggedUser);
            notification.IsRead = true;
            this.Data.SaveChanges();

            return this.Content("Invitation accepted", "text/html");
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeclineInvitation(ResolveNotificationBindingModel model)
        {
            if (!this.ModelState.IsValid || model == null)
            {
                throw new HttpException();
            }

            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.Find(loggedUserId);
            var notification = this.Data.Notifications.Find(model.NotificationId);
            var contest = this.Data.Contests.Find(model.ContestId);

            contest.Invited.Remove(loggedUser);
            contest.Declined.Add(loggedUser);
            notification.IsRead = true;
            this.Data.SaveChanges();

            return this.Content("Invitation declined", "text/html");
        }
    }
}