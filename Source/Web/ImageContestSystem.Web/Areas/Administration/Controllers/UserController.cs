namespace ImageContestSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using ImageContestSystem.Web.Controllers;
    using ImageContestSystem.Web.Models.ViewModels;
    using Models;

    [Authorize(Roles = "Administrator")]
    public class UserController : BaseController
    {
        public ActionResult Get()
        {
            var users = this.Data.Users.All()
                .Take(10)
                .Select(u => new UserViewModel
                {
                    UserId = u.Id,
                    Username = u.UserName,
                    OwnContest = u.CreatedContests.Count,
                    ParticipatedInContests = u.ParticipatedIn.Count,
                    UploadedPhotos = u.UploadedPictures.Count,
                    Contests = u.ParticipatedIn        
                        .OrderByDescending(c => c.CreatedOn)
                        .Select(c => new ContestViewModel
                        {
                            Id = c.Id,
                            Title = c.Title
                        })
                        .ToList(),
                    OwnContests = u.CreatedContests
                        .OrderByDescending(c => c.CreatedOn)
                            .Select(c => new ContestViewModel
                            {
                                Id = c.Id,
                                Title = c.Title
                            })
                            .ToList()
                })
                .ToList();

            return this.PartialView("_ShowUsersPartial", users);
        }

        public ActionResult Remove(int userId, int contestId)
        {
            var user = this.Data.Users.Find(userId);
            var contest = this.Data.Contests.Find(contestId);

            contest.Participants.Remove(user);
            user.ParticipatedIn.Remove(contest);
            this.Data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
	}
}