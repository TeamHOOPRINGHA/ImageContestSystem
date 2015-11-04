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
        public ActionResult Index(int id = 1, int pageSize = 5)
        {
            var loggedUserId = this.User.Identity.GetUserId();

            if (loggedUserId != null)
            {
                var contests = this.Data.Contests.All()
                .Where(c => c.ClosesOn > DateTime.Now && c.Participants.Any(u => u.Id == loggedUserId))
                .ToList();

                var paged = new HomeViewModel
                {
                    PageCount = contests.Count % pageSize == 0
                            ? contests.Count / pageSize
                            : contests.Count / pageSize + 1,
                    PageSize = pageSize,
                    CurrentPage = id,
                    Contests = contests
                        .OrderByDescending(c => c.CreatedOn)
                        .Skip((id - 1) * pageSize)
                        .Take(pageSize)
                        .Select(c => new ContestParticipantViewModel
                        {
                            Id = c.Id,
                            Title = c.Title,
                            Creator = c.Creator.UserName,
                            CurrentLeader = c.CurrentLeader == null ? "No leader yet" : c.CurrentLeader.UserName,
                            Pictures = c.Pictures
                                .Take(4)
                                .Select(p => new PhotoViewModel()
                                {
                                    Id = p.Id,
                                    Author = p.Author.UserName,
                                    ContestId = c.Id,
                                    Location = p.LocationPath,
                                    Votes = p.Votes.Count,
                                    HasVoted = p.Votes.Any(v => v.VoterId == loggedUserId)
                                })
                                .ToList()
                        }).ToList()
                };

                return View(paged);
            }


            var contestsForAnonymUser = this.Data.Contests.All()
                .OrderByDescending(c => c.CreatedOn)
                .ToList();


            var result = new HomeViewModel
            {
                PageCount = contestsForAnonymUser.Count % pageSize == 0
                            ? contestsForAnonymUser.Count / pageSize
                            : contestsForAnonymUser.Count / pageSize + 1,
                PageSize = pageSize,
                CurrentPage = id,
                Contests = contestsForAnonymUser
                    .Skip((id - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new ContestParticipantViewModel
                    {
                        Title = c.Title,
                        CurrentLeader = c.CurrentLeader == null ? "No leader yet" : c.CurrentLeader.UserName,
                        Pictures = c.Pictures
                            .Take(4)
                            .Select(p => new PhotoViewModel
                            {
                                Id = p.Id,
                                Location = p.LocationPath,
                                Author = p.Author.UserName,
                                Votes = p.Votes.Count
                            }).ToList(),
                        Description = c.Description
                    }).ToList()
            };

            return View(result);
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
                    Id = p.Id,
                    Location = p.LocationPath,
                    Author = p.Author.UserName,
                    HasVoted = p.Votes.Any(v => v.VoterId == loggedUserId),
                    Votes = p.Votes.Count
                })
                .ToList();

            return View(myPhotos);
         }
    }
}