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
    using System.Web;
    using ImageContestSystem.Data.UnitOfWork;
    using Data.Models.Enums;

    public class ContestController : BaseController
    {
        public ContestController(IImageContestData data)
            : base(data)
        {
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

                return RedirectToAction("MyContests", "Home");
            }
            
            return View();
        }

        [Authorize]
        public ActionResult Dismiss(int id)
        {
            var contest = this.Data.Contests.All().Where(c => c.Id == id).FirstOrDefault();

            if (contest == null)
            {
                return new HttpNotFoundResult("Contest not found.");
            }

            contest.IsDismissed = true;
            this.Data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Finalize(int id)
        {
            var contest = this.Data.Contests.All().Where(c => c.Id == id).FirstOrDefault();

            if (contest == null)
            {
                return new HttpNotFoundResult("Contest not found.");
            }

            contest.IsFinalized = true;

            //Determine winner or winners
            //If there is a single winner
            if (contest.RewardStrategy == RewardStrategy.SingleWinner)
            {
                var winnerId = contest.Pictures.OrderByDescending(p => p.Votes.Count).FirstOrDefault().AuthorId;
                contest.WinnerId = winnerId;
            }

            this.Data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult View(int id)
        {
            var loggedUserId = User.Identity.GetUserId();
            var user = this.Data.Users.All().FirstOrDefault(u => u.Id == loggedUserId);
            var contest = this.Data.Contests.Find(id);
                
            if (loggedUserId != null && contest.Participants.Any(p => p.Id == loggedUserId))
            {
                var contestToReturn = new ContestParticipantViewModel
                {
                    Id = contest.Id,
                    Title = contest.Title,
                    Description = contest.Description,
                    Creator = contest.Creator.UserName,
                    CreatedOn = contest.CreatedOn,
                    ClosesOn = contest.ClosesOn,
                    IsDismissed = contest.IsDismissed,
                    IsFinalized = contest.IsFinalized,
                    Pictures = contest.Pictures.Take(10).Select(p => new PhotoViewModel
                    {
                        Id = p.Id,
                        Author = p.Author.UserName,
                        Location = p.LocationPath,
                        Votes = p.Votes.Count
                    }).ToList(),
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

        
        [Authorize]
        public ActionResult ViewAll(int id = 1, int pageSize = 5)
        {
            var loggedUserId = User.Identity.GetUserId();
            
            var contests = this.Data.Contests.All()
                .Where(c => c.IsDismissed == false && c.IsFinalized == false && ((c.ClosesOn > DateTime.Now || c.ClosesOn == null) && c.Participants.Count(p => p.Id == loggedUserId) == 0))
                .ToList();

            var model = new BrowseContestsViewModel
            {
                CurrentPage = id,
                PageSize = pageSize,
                PageCount = contests.Count % pageSize == 0
                        ? contests.Count / pageSize
                        : contests.Count / pageSize + 1,
                Contests = contests.OrderByDescending(c => c.CreatedOn)
                    .Skip((id - 1) * pageSize)
                    .Take(pageSize)
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
                    }).ToList()
            };
            
            return View(model);
        }

        public ActionResult ViewPast()
        {
            var contests = this.Data.Contests.All()
                .Where(c => c.ClosesOn <= DateTime.Now || c.IsDismissed || c.IsFinalized)
                .OrderByDescending(c => c.ClosesOn)
                .ProjectTo<ContestViewModel>()
                .ToList();

            return View(contests);
        }

        [Authorize]
        public ActionResult Edit(EditContestViewModel model)
        {
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No data");
            }

            if (!this.ModelState.IsValid)
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

            return RedirectToAction("MyContests", "Home");
        }
    }
}