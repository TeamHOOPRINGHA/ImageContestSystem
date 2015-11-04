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

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, ErrorMessage = "Description length should be less than 100")]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        [Display(Name = "Voting strategy")]
        [Required(ErrorMessage = "Voting Strategy is required.")]
        public VotingStrategy VotingStrategy { get; set; }

        [Display(Name = "Reward strategy")]
        [Required(ErrorMessage = "Reward Strategy is required.")]
        public RewardStrategy RewardStrategy { get; set; }

        [Display(Name = "Participation strategy")]
        [Required(ErrorMessage = "Participation Strategy is required.")]
        public ParticipationStrategy ParticipationStrategy { get; set; }

        [Display(Name = "Deadline strategy")]
        [Required(ErrorMessage = "Deadline Strategy is required.")]
        public DeadlineStrategy DeadlineStrategy { get; set; }

        [Display(Name = "Closes on")]
        public DateTime? ClosesOn { get; set; }

        [Display(Name = "Participants")]
        [Range(5, 100)]
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