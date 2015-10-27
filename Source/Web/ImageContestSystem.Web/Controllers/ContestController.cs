namespace ImageContestSystem.Web.Controllers
{
    using Data.Models;
    using Microsoft.AspNet.Identity;
    using Models.ViewModels;
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;

    public class ContestController : BaseController
    {
        public ActionResult Index()
        {
            var loggedUserId = User.Identity.GetUserId();
            var user = this.Data.Users.All().FirstOrDefault(u => u.Id == loggedUserId);

            var contests = this.Data.Contests
                .All()
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

                return View("ViewByAnonymous", output);
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

                return View("ViewByAuthorized", output);
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
                    ParticipationStrategy = model.ParticipationStrategy
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
    }
}