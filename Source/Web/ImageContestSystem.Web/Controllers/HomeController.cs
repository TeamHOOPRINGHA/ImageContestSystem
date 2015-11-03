namespace ImageContestSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using ImageContestSystem.Web.Models.ViewModels;
    using AutoMapper.QueryableExtensions;

    public class HomeController : BaseController
    {
        public ActionResult Index(int id = 1, int pageSize = 3)
        {
            var loggedUserId = this.User.Identity.GetUserId();

            var contests = this.Data.Contests.All()
                .Where(c => c.ClosesOn > DateTime.Now && c.Participants.Any(u => u.Id == loggedUserId))
                .ToList();

            var paginationContests = new HomeViewModel
            {
                PageCount = contests.Count / pageSize,
                PageSize = pageSize,
                CurrentPage = id,
                Contests = contests
                    .OrderByDescending(c => c.CreatedOn)
                    .Skip((id - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new ContestParticipantViewModel()
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Creator = c.Creator.UserName,
                        Pictures = c.Pictures
                            .Take(4)
                            .Select(p => new PhotoViewModel()
                            {
                                Author = p.Author.UserName,
                                ContestId = c.Id,
                                Location = p.LocationPath
                            })
                            .ToList()
                    }).ToList()
            };

            return View(paginationContests);
        }

        [Authorize]
        public ActionResult MyContests()
        {
            var loggedUserId = this.User.Identity.GetUserId();

            var myContests = this.Data.Contests.All()
                .Where(c => c.CreatorId == loggedUserId)
                .Take(6)
                .OrderByDescending(c => c.ClosesOn)
                .ProjectTo<ContestViewModel>()
                .ToList();

            return this.View(myContests);
        }

        [Authorize]
        [HttpGet]
        public ActionResult MyPhotos()
        {
            var loggedUserId = this.User.Identity.GetUserId();

            var myPhotos = this.Data.Pictures.All()
                .Where(u => u.AuthorId == loggedUserId)
                .Take(10)
                .Select(p => new PhotoViewModel
                {
                    Location = p.LocationPath,
                    Author = p.Author.UserName
                })
                .ToList();

            return View(myPhotos);
         }
    }
}