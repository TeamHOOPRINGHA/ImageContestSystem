namespace ImageContestSystem.Web.Models.ViewModels
{
    using Data.Models;
    using Infrastructure.Mapping;
    using Data.Models.Enums;
    using System;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;

    public class ContestViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        [Display(Name = "Voting strategy")]
        public VotingStrategy VotingStrategy { get; set; }

        [Display(Name = "Reward strategy")]
        public RewardStrategy RewardStrategy { get; set; }

        [Display(Name = "Participation strategy")]
        public ParticipationStrategy ParticipationStrategy { get; set; }

        [Display(Name = "Deadline strategy")]
        public DeadlineStrategy DeadlineStrategy { get; set; }

        [Display(Name = "Closes on")]
        public DateTime? ClosesOn { get; set; }

        [Display(Name = "Participants")]
        public int? NumberOfAllowedParticipants { get; set; }

        public int CountOfParticipants { get; set; }

        public bool HasParticipated { get; set; }

        public bool IsDismissed { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Contest, ContestViewModel>()
                .ForMember(c => c.CountOfParticipants, opt => opt.MapFrom(contest => contest.Participants.Count));
        }
    }
}