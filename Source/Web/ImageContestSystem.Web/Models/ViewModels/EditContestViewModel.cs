namespace ImageContestSystem.Web.Models.ViewModels
{
    using ImageContestSystem.Data.Models;
    using ImageContestSystem.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;

    public class EditContestViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, ErrorMessage = "Description length should be less than 100")]
        public string Description { get; set; }

        public DateTime? ClosesOn { get; set; }

        public VotingStrategy VotingStrategy { get; set; }

        //public RewardStrategy RewardStrategy { get; set; }

        //public ParticipationStrategy ParticipationStrategy { get; set; }

        //public DeadlineStrategy DeadlineStrategy { get; set; }

        public int? NumberOfAllowedParticipants { get; set; }

        public static Expression<Func<Contest, EditContestViewModel>> Create
        {
            get
            {
                return c => new EditContestViewModel
                {
                    Id = c.Id,
                    Description = c.Description,
                    ClosesOn = c.ClosesOn,
                    //DeadlineStrategy = c.DeadlineStrategy,
                    NumberOfAllowedParticipants = c.NumberOfAllowedParticipants,
                    //ParticipationStrategy = c.ParticipationStrategy,
                    //RewardStrategy = c.RewardStrategy,
                    VotingStrategy = c.VotingStrategy
                };
            }
        }
    }
}