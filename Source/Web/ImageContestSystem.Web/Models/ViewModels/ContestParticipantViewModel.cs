namespace ImageContestSystem.Web.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using Data.Models;

    public class ContestParticipantViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Creator { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ClosesOn { get; set; }

        public int Pictures { get; set; }

        public bool HasAddedPhoto { get; set; }

        public static Expression<Func<Contest, ContestParticipantViewModel>> Create
        {
            get
            {
                return c => new ContestParticipantViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Creator = c.Creator.UserName,
                    CreatedOn = c.CreatedOn,
                    ClosesOn = c.ClosesOn,
                    Pictures = c.Pictures.Count
                };
            }
        }
    }
}