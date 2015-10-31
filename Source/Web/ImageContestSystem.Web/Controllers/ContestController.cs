namespace ImageContestSystem.Web.Controllers
{
    using Data.Models;
    using Microsoft.AspNet.Identity;
    using Models.ViewModels;
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using System.Net;

    public class ContestController : BaseController
    {
        public ActionResult Index()
        {
            var loggedUserId = User.Identity.GetUserId();
            var user = this.Data.Users.All().FirstOrDefault(u => u.Id == loggedUserId);

            var contests = this.Data.Contests
                .All()
                .Where(c => c.ClosesOn > DateTime.Now)
                .OrderByDescending(c => c.CreatedOn)
                .Take(5);

            if (user == null)
            {
                var output = contests
                    .Select(c => new ContestViewModel()
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Description = c.Description
                    }).ToList();

                return View("ViewAllByAnonymous", output);
            }
            else
            {
                var output = contests
                    .Select(c => new ContestViewModel()
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Description = c.Description,
                        CountOfParticipants = c.Participants.Count,
                        ClosesOn = c.ClosesOn,
                        NumberOfAllowedParticipants = c.NumberOfAllowedParticipants,
                        ParticipationStrategy = c.ParticipationStrategy,
                        HasParticipated = c.Participants.Any(p => p.Id == loggedUserId)
                    }).ToList();

                return View("ViewAllByAuthorized", output);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ContestViewModel model)
        {
            var loggedUserId = User.Identity.GetUserId();
            var user = this.Data.Users.All().FirstOrDefault(u => u.Id == loggedUserId);

            if (user != null)
            {
                if (model == null)
                {
                    return View(model);
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }
            
                var newContest = new Contest()
                {
                    Title = model.Title,
                    Description = model.Description,
                    CreatorId = loggedUserId,
                    CreatedOn = DateTime.Now,
                    ClosesOn = model.ClosesOn,
                    NumberOfAllowedParticipants = model.NumberOfAllowedParticipants,
                    ParticipationStrategy = model.ParticipationStrategy,
                    DeadlineStrategy = model.DeadlineStrategy,
                    RewardStrategy = model.RewardStrategy,
                    VotingStrategy = model.VotingStrategy
                };
            
                user.CreatedContests.Add(newContest);
                this.Data.SaveChanges();
                this.TempData["SuccessMessage"] = "Contest successfully created!";

                return RedirectToAction("Index", "Contest");
            }
            
            return View();
        }

        public ActionResult View(int id)
        {
            var loggedUserId = User.Identity.GetUserId();
            var user = this.Data.Users.All().FirstOrDefault(u => u.Id == loggedUserId);
            var contest = this.Data.Contests.Find(id);
                
            if (loggedUserId != null && contest.Participants.Any(p => p.Id == loggedUserId))
            {
                var contestToReturn = new ContestParticipantViewModel()
                {
                    Id = contest.Id,
                    Title = contest.Title,
                    Description = contest.Description,
                    Creator = contest.Creator.UserName,
                    CreatedOn = contest.CreatedOn,
                    ClosesOn = contest.ClosesOn,
                    Pictures = contest.Pictures.Count,
                    HasAddedPhoto = contest.Pictures.Any(p => p.AuthorId == loggedUserId)
                };

                return View("ViewByParticipant", contestToReturn);
            }

            var searchedContest = this.Data.Contests
                .All()
                .Where(c => c.Id == id)
                .ProjectTo<ContestViewModel>()
                .FirstOrDefault();
            
            if (searchedContest == null)
            {
                return View("Error");
            }

            if (user != null)
            {
                return View("ViewByAuthorized", searchedContest);
            }

            return View("ViewByAnonymous", searchedContest);
        }

        public ActionResult ViewPast()
        {
            var contests = this.Data.Contests.All()
                .Where(c => c.ClosesOn <= DateTime.Now)
                .OrderByDescending(c => c.ClosesOn)
                .ProjectTo<ContestViewModel>()
                .ToList();

            return View(contests);
        }

        [Authorize]
        public ActionResult Edit(EditContestViewModel model)
        {
            if (model == null || !this.ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid data");
            }
 
            var editedContest = this.Data.Contests.Find(model.Id);
 
            if (editedContest == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "404");
            }
 
            if(model.ClosesOn != null)
            {
                editedContest.ClosesOn = model.ClosesOn; 
            }
 
            if (model.Description != null)
            {
                editedContest.Description = model.Description;
            }
 
            if (model.NumberOfAllowedParticipants != null)
            {
                editedContest.NumberOfAllowedParticipants = model.NumberOfAllowedParticipants;
            }
            
            editedContest.VotingStrategy = model.VotingStrategy;
            this.Data.SaveChanges();
 
            return RedirectToAction("MyContests", "User");
        }
    }
}